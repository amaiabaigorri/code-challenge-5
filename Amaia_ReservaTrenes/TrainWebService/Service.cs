﻿namespace TrainWebService
{
    using CrossCutting.Constants;
    using CrossCutting.Resources;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public static class Service
    {
        private static HttpClient client;

        public static HttpClient InitializeHttpClient()
        {
            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }
            catch (Exception)
            {
                throw new Exception(Exceptions.ConnectionError);
            }
        }


    }
}
