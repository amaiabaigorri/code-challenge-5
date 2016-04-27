﻿namespace Amaia_ReservaTrenes.TrainFactory
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Express2000_Train : ITrainInformation
    {
        public async Task GetInformation(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:9600/data_for_train/express_2000");

            if (response.IsSuccessStatusCode)
            {
                var seats = await response.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, SeatProperty>>>(seats);
            }
        }
    }
}
