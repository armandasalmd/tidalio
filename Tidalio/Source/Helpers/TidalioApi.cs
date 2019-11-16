using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Android.Net;

namespace Tidalio
{
    /// <summary>
    /// Helper class to handle Web API requests.
    /// Used to save user data on cloud
    /// </summary>
    class TidalioApi
    {
        private static TidalioApi instance;
        private static HttpClient client;

        private TidalioApi() => client = new HttpClient(new AndroidClientHandler());

        public static TidalioApi GetInstance()
        {
            if (instance == null)
                instance = new TidalioApi();
            return instance;
        }

        /// <summary>
        /// Get user saved locations from web API
        /// </summary>
        /// <param name="user_email">User email</param>
        /// <returns>List of user Location's</returns>
        public List<Location> FetchLocations(string user_email)
        {
            string response = GetInstance().GetLocationsJSON(user_email);
            List<Newton.LocationNewton> modelList = JsonConvert.DeserializeObject<List<Newton.LocationNewton>>(response);
            return NewtonModelsConverter.ListLocationNewton_ToList_Location(modelList);
        }

        /// <summary>
        /// Get user saved forecast cards from web API
        /// </summary>
        /// <param name="user_email">User email</param>
        /// <returns>List of ForecastCard's</returns>
        public List<ForecastCard> FetchForecasts(string user_email)
        {
            string response = GetInstance().GetForecastsJSON(user_email);
            List<Newton.ForecastCardNewton> modelList = JsonConvert.DeserializeObject<List<Newton.ForecastCardNewton>>(response);
            return NewtonModelsConverter.ListForecasCardtNewton_ToList_ForecastCard(modelList);
        }

        /// <summary>
        /// Makes a request to fetch JSON data to web API
        /// </summary>
        /// <param name="user_email">User email</param>
        /// <returns>JSON string</returns>
        public string GetLocationsJSON(string user_email)
        {
            string callUrl = $"{Constants.TidalioDomain}/locations/{user_email}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        /// <summary>
        /// Makes a POST request to web API under domain/locations path
        /// </summary>
        /// <param name="user_email">User email</param>
        /// <param name="loc">Location model instance</param>
        public async void PostLocationAsync(string user_email, Location loc)
        {
            string callUrl = $"{Constants.TidalioDomain}/locations";
            Newton.LocationNewton bodyObj = NewtonModelsConverter.Location_To_LocationNewton(user_email, loc);
            var body = new StringContent(JsonConvert.SerializeObject(bodyObj), Encoding.UTF8, "application/json");
            await client.PostAsync(callUrl, body);
        }

        /// <summary>
        /// Makes a DELETE request to web API under domain/locations/:email/:address path
        /// </summary>
        /// <param name="user_email">User email</param>
        /// <param name="loc">Location model instance</param>
        public async void DeleteLocationAsync(string user_email, Location loc)
        {
            string callUrl = $"{Constants.TidalioDomain}/locations/{user_email}/{loc.Address}";
            await client.DeleteAsync(callUrl);
        }

        /// <summary>
        /// Makes a request to web API to fetch saved ForecastCard data
        /// </summary>
        /// <param name="user_email">Location model instance</param>
        /// <returns>JSON string</returns>
        public string GetForecastsJSON(string user_email)
        {
            string callUrl = $"{Constants.TidalioDomain}/forecastcards/{user_email}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        /// <summary>
        /// Saves ForecastCard data linked to given user in web API
        /// </summary>
        /// <param name="user_email">Location model instance</param>
        /// <param name="card">ForecastCard model instance</param>
        public async void PostCardAsync(string user_email, ForecastCard card)
        {
            string callUrl = $"{Constants.TidalioDomain}/forecastcards";
            Newton.ForecastCardNewton bodyObj = NewtonModelsConverter.ForecastCard_To_ForecastCardNewton(user_email, card);
            var body = new StringContent(JsonConvert.SerializeObject(bodyObj), Encoding.UTF8, "application/json");
            await client.PostAsync(callUrl, body);
        }

        /// <summary>
        /// Deletes ForecastCard object in web API
        /// </summary>
        /// <param name="user_email">User email</param>
        /// <param name="card">ForecastCard model instance</param>
        public async void DeleteCardAsync(string user_email, ForecastCard card)
        {
            string queryDate = Functions.EscapeBackslash(card.DateFormated);
            string callUrl = $"{Constants.TidalioDomain}/forecastcards/{user_email}/{card.Location}/{queryDate}";
            await client.DeleteAsync(callUrl);
        }
    }
}