﻿namespace Amaia_ReservaTrenes.ReservationHandler
{
    using CrossCutting.Constants;
    using CrossCutting.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using TrainWebService;
    using Utils;

    public class CoachHandler : Handler
    {
        HandlerUtils utils;
        Service service;

        public CoachHandler(HandlerUtils _utils, Service _service)
        {
            this.utils = _utils;
            this.service = _service;
        }

        public override void HandleReservationRequest(Dictionary<string, SeatProperty> trainInfo, ReserveModel reservationReference, int numberSeats)
        {
            bool hasCoachHandleReservation = false;
            foreach (var coachInfo in trainInfo.Select(x => x.Value.coach).Distinct())
            {
                hasCoachHandleReservation = this.HandleEachCoachReservation(trainInfo.Where(x => x.Value.coach.Equals(coachInfo)), reservationReference, numberSeats, coachInfo);
                if (hasCoachHandleReservation)
                {
                    break;
                }
            }

            if (!hasCoachHandleReservation)
            {
                successor.HandleReservationRequest(trainInfo, reservationReference, numberSeats);
            }
        }

        bool HandleEachCoachReservation(IEnumerable<KeyValuePair<string, SeatProperty>> coachInfo, ReserveModel reservationReference, int numberSeats, string coach) {

            try
            {
                this.CheckIfIsMoreThan70PercentBooking(coachInfo, numberSeats);
                reservationReference.seats = this.utils.BookSeats(coachInfo, coach, numberSeats);
                this.utils.DoReservation(reservationReference, this.service);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void CheckIfIsMoreThan70PercentBooking(IEnumerable<KeyValuePair<string, SeatProperty>> coachInfo, int numberSeats)
        {
            var bookSeats = coachInfo.Where(x => !string.IsNullOrEmpty(x.Value.booking_reference)).ToDictionary(x => x.Key, x => x.Value).Count();
            var percentageOfBooking = (bookSeats + numberSeats) / Convert.ToDouble(coachInfo.Count());
            if (percentageOfBooking > Constants.Percentage)
            {
                throw new Exception();
            }
        }
    }
}
