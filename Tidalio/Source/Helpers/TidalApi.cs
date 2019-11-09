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


        public string GetStations()
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        public async Task<string> GetStationsAsync()
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }

        public string GetStation(string stationId)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        public async Task<string> GetStationAsync(string stationId)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }

        public string GetTidalEvents(string stationId, int duration = 1)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}/TidalEvents?duration={duration}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        public async Task<string> GetTidalEventsAsync(string stationId, int duration = 1)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}/TidalEvents?duration={duration}";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }
        // Code start here

    }
}