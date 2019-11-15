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

        public string GetWaterInfo(string stationId, int addDays = 0, int hour = 0)
        {
            List<TidalEvent> eventList = FetchEvents(stationId, addDays + 1);
            eventList = FilterEventsByLastDate(eventList); // filtering by date
            // List contains different time data for station with stationID
            int matchedTimeEventId = eventList.Count - 1; // select last tidal record as backup
            for (int i = 0; i < eventList.Count; i++)
            {
                if (eventList[i].GetDateTimeObj().Hour >= hour)
                {
                    matchedTimeEventId = i;
                    break;
                }
            }
            if (matchedTimeEventId >= 0 && matchedTimeEventId < eventList.Count)
                return eventList[matchedTimeEventId].ToString();
            // Default cases
            return string.Empty;
        }

        public List<TidalEvent> FetchEvents(string stationId, int duration = 1)
        {
            string response = GetInstance().GetTidalEventsJSON(stationId, duration);
            List<Newton.TidalEventNewton> eventList = JsonConvert.DeserializeObject<List<Newton.TidalEventNewton>>(response);
            return NewtonModelsConverter.TidalEventNewton_ToList_TidalEvent(eventList, stationId);
        }

        public List<TidalEvent> FilterEventsByLastDate(List<TidalEvent> dataList)
        {
            // a data list events are different time, 
            // this function takes latest date and leaves only same date events
            if (dataList.Count > 1)
            {
                List<TidalEvent> resultList = new List<TidalEvent>();
                TidalEvent lastEven = dataList.Last();
                string extractDate = lastEven.DateTime.Substring(0, 10); // i.e. 2019-11-12
                foreach(TidalEvent t in dataList)
                {
                    if (t.DateTime.StartsWith(extractDate))
                        resultList.Add(t);
                }
                return resultList;
            }
            else return dataList;
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