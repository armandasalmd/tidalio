using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Android.Net;

namespace Tidalio
{
    class TidalioApi
    {
        private static TidalioApi instance;
        private static HttpClient client;
        //static readonly HttpClient client = new HttpClient();
        private TidalioApi()
        {
            client = new HttpClient(new AndroidClientHandler());
            //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.TidalKey);
        }
        public static TidalioApi GetInstance()
        {
            if (instance == null)
                instance = new TidalioApi();
            return instance;
        }

        public List<Location> FetchLocations(string user_email)
        {
            string response = GetInstance().GetLocationsJSON(user_email);
            List<Newton.LocationNewton> modelList = JsonConvert.DeserializeObject<List<Newton.LocationNewton>>(response);
            return NewtonModelsConverter.ListLocationNewton_ToList_Location(modelList);
        }

        public List<ForecastCard> FetchForecasts(string user_email)
        {
            string response = GetInstance().GetForecastsJSON(user_email);
            List<Newton.ForecastCardNewton> modelList = JsonConvert.DeserializeObject<List<Newton.ForecastCardNewton>>(response);
            return NewtonModelsConverter.ListForecasCardtNewton_ToList_ForecastCard(modelList);
        }

        public string GetLocationsJSON(string user_email)
        {
            string callUrl = $"{Constants.TidalioDomain}/locations/{user_email}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        public async void PostLocationAsync(string user_email, Location loc)
        {
            string callUrl = $"{Constants.TidalioDomain}/locations";
            Newton.LocationNewton bodyObj = NewtonModelsConverter.Location_To_LocationNewton(user_email, loc);
            var body = new StringContent(JsonConvert.SerializeObject(bodyObj), Encoding.UTF8, "application/json");
            await client.PostAsync(callUrl, body);
        }

        public async void DeleteLocationAsync(string user_email, Location loc)
        {
            string callUrl = $"{Constants.TidalioDomain}/locations/{user_email}/{loc.Address}";
            await client.DeleteAsync(callUrl);
        }

        public string GetForecastsJSON(string user_email)
        {
            string callUrl = $"{Constants.TidalioDomain}/forecastcards/{user_email}";
            var result = Task.Run(() => client.GetAsync(callUrl)).Result;
            var resBody = Task.Run(() => result.Content.ReadAsStringAsync()).Result;
            return resBody;
        }

        public async void PostCardAsync(string user_email, ForecastCard card)
        {
            string callUrl = $"{Constants.TidalioDomain}/forecastcards";
            Newton.ForecastCardNewton bodyObj = NewtonModelsConverter.ForecastCard_To_ForecastCardNewton(user_email, card);
            var body = new StringContent(JsonConvert.SerializeObject(bodyObj), Encoding.UTF8, "application/json");
            await client.PostAsync(callUrl, body);
        }

        public async void DeleteCardAsync(string user_email, ForecastCard card)
        {
            string queryDate = Functions.EscapeBackslash(card.DateFormated);
            string callUrl = $"{Constants.TidalioDomain}/forecastcards/{user_email}/{card.Location}/{queryDate}";
            await client.DeleteAsync(callUrl);
        }
    }
}