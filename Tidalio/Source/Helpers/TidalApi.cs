using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using Xamarin.Android.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tidalio
{
    public class TidalApi
    {

        private static TidalApi instance;
        private static HttpClient client;
        //static readonly HttpClient client = new HttpClient();
        private TidalApi()
        {
            client = new HttpClient(new AndroidClientHandler());
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.TidalKey);
        }
        public static TidalApi GetInstance()
        {
            if (instance == null)
                instance = new TidalApi();
            return instance;
        }

        public string GetWaterInfo(string stationId)
        {
            List<TidalEvent> eventList = FetchEvents(stationId);
            // List contains different time data for station with stationID
            if (eventList.Count > 0)
                return eventList[0].ToString();
            return string.Empty;
        }

        public List<TidalEvent> FetchEvents(string stationId, int duration = 1)
        {
            string response = GetInstance().GetTidalEventsJSON(stationId, duration);
            List<Newton.TidalEventNewton> eventList = JsonConvert.DeserializeObject<List<Newton.TidalEventNewton>>(response);
            return NewtonModelsConverter.TidalEventNewton_ToList_TidalEvent(eventList, stationId);
        }

        public List<TidalStation> FetchStations()
        {
            string response = GetInstance().GetStationsJSON();
            Newton.TidalStationsNewton model = JsonConvert.DeserializeObject<Newton.TidalStationsNewton>(response);
            return NewtonModelsConverter.TidalStationsNewton_ToList_TidalStation(model);
        }

        public string GetStationsJSON()
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        public async Task<string> GetStationsJSONAsync()
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }

        public string GetStationJSON(string stationId)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        public async Task<string> GetStationJSONAsync(string stationId)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }

        public string GetTidalEventsJSON(string stationId, int duration = 1)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}/TidalEvents?duration={duration}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        public async Task<string> GetTidalEventsJSONAsync(string stationId, int duration = 1)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}/TidalEvents?duration={duration}";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }
        // Code start here

    }
}