using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xamarin.Android.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tidalio
{
    /// <summary>
    /// Helper class to handle Tidal API requests
    /// </summary>
    public class TidalApi
    {
        private static TidalApi instance;
        private static HttpClient client;

        /// <summary>
        /// Singleton design pattern constructor
        /// </summary>
        private TidalApi()
        {
            client = new HttpClient(new AndroidClientHandler());
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.TidalKey);
        }

        /// <summary>
        /// Get single instance of this class
        /// </summary>
        /// <returns>Singleton instance</returns>
        public static TidalApi GetInstance()
        {
            if (instance == null)
                instance = new TidalApi();
            return instance;
        }

        /// <summary>
        /// Fetches API data and selects the best time record for given station
        /// </summary>
        /// <param name="stationId">Station id</param>
        /// <param name="addDays">Days count to add to today</param>
        /// <param name="hour">Hours to add to current hour</param>
        /// <returns>Returns the best matched(to time) record as a string</returns>
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

        /// <summary>
        /// Fetches JSON data from API and then converts it to regular model
        /// </summary>
        /// <param name="stationId">Station id</param>
        /// <param name="duration">Days count to forecasts upfront</param>
        /// <returns>List of TidalEvents</returns>
        public List<TidalEvent> FetchEvents(string stationId, int duration = 1)
        {
            string response = GetInstance().GetTidalEventsJSON(stationId, duration);
            List<Newton.TidalEventNewton> eventList = JsonConvert.DeserializeObject<List<Newton.TidalEventNewton>>(response);
            return NewtonModelsConverter.TidalEventNewton_ToList_TidalEvent(eventList, stationId);
        }

        /// <summary>
        /// Selects last date and other/non-matching dates
        /// </summary>
        /// <param name="dataList">List to filter</param>
        /// <returns>Filtered list</returns>
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

        /// <summary>
        /// Fetches JSON data from API an converts to regular model
        /// </summary>
        /// <returns>List of TidalStation's</returns>
        public List<TidalStation> FetchStations()
        {
            string response = GetInstance().GetStationsJSON();
            Newton.TidalStationsNewton model = JsonConvert.DeserializeObject<Newton.TidalStationsNewton>(response);
            return NewtonModelsConverter.TidalStationsNewton_ToList_TidalStation(model);
        }

        /// <summary>
        /// Performs a request and gets JSON response
        /// </summary>
        /// <returns>JSON string</returns>
        public string GetStationsJSON()
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        /// <summary>
        /// Performs async request and gets JSON response
        /// </summary>
        /// <returns>JSON string</returns>
        public async Task<string> GetStationsJSONAsync()
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }

        /// <summary>
        /// Gets informations about the station
        /// </summary>
        /// <param name="stationId">Station id</param>
        /// <returns>JSON string</returns>
        public string GetStationJSON(string stationId)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        /// <summary>
        /// Gets information about the station ASYNC
        /// </summary>
        /// <param name="stationId">Station id</param>
        /// <returns>JSON string</returns>
        public async Task<string> GetStationJSONAsync(string stationId)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }

        /// <summary>
        /// Gets tidal event informaton for particular station
        /// </summary>
        /// <param name="stationId">Station id</param>
        /// <param name="duration">Days upfront</param>
        /// <returns>JSON string</returns>
        public string GetTidalEventsJSON(string stationId, int duration = 1)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}/TidalEvents?duration={duration}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        /// <summary>
        /// Gets tidal event information for particular station ASYNC
        /// </summary>
        /// <param name="stationId">Station id</param>
        /// <param name="duration">Days upfront</param>
        /// <returns>JSON string</returns>
        public async Task<string> GetTidalEventsJSONAsync(string stationId, int duration = 1)
        {
            string callUrl = $"{Constants.TidalDomain}/uktidalapi/api/V1/Stations/{stationId}/TidalEvents?duration={duration}";
            var result = await client.GetAsync(callUrl);
            var resBody = await result.Content.ReadAsStringAsync();
            return resBody;
        }
    }
}