using System;
using System.Threading.Tasks;
using DarkSkyApi;
using Xamarin.Essentials;

namespace Tidalio
{
    /// <summary>
    /// A model that is used to depict forecast information inside the card component
    /// </summary>
    public class ForecastCard
    {
        private DateTime date;
        private string strDate;
        private bool saved;
        private string icon, location, 
            temperature, summary, 
            humidity, windSpeed, 
            windDirection, waterLevel;

        /// <summary>
        /// Gives date in gb-UK locality format
        /// </summary>
        public string DateFormated
        {
            get {
                if (strDate != null)
                    return strDate;
                else
                    return date.ToString("dd/MM/yyyy HH:mm");
            }
        }
        public DateTime Date { get { return date; } }
        public bool IsSaved { get { return saved; } set { saved = value; } }
        public string Icon { get { return icon; } }
        public string Location { get { return location; } }
        public string Temperature { get { return temperature; } }
        public string Summary { get { return summary; } }
        public string Humidity { get { return humidity; } }
        public string WindSpeed { get { return windSpeed; } }
        public string WindDirection { get { return windDirection; } }
        
        /// <summary>
        /// Gives water level in meters or unavailable if object is not set
        /// </summary>
        public string WaterLevel {
            get {
                if (waterLevel == "0m")
                    return "unavailable";
                else
                    return waterLevel;
            }
        }

        /// <summary>
        /// Initialize model with default values
        /// </summary>
        public ForecastCard()
        {
            // Default values
            date = DateTime.Now;
            saved = false;
            icon = "clear-day";
            location = "unknown";
            temperature = "0°C";
            summary = "unknown";
            humidity = "0%";
            windSpeed = "0m/s";
            windDirection = "0°";
            waterLevel = "0m";
        }

        /// <summary>
        /// Initialize model converting Newton Model respectively
        /// </summary>
        /// <param name="d">Newton model object</param>
        public ForecastCard(Newton.ForecastCardNewton d)
        {
            strDate = d.forecast_date;
            saved = true;
            icon = d.icon;
            location = d.forecast_location;
            temperature = d.temperature;
            summary = d.summary;
            humidity = d.humidity;
            windSpeed = d.wind_speed;
            windDirection = d.wind_direction;
            waterLevel = d.water_level;
        }

        /// <summary>
        /// Initialize model using given values
        /// </summary>
        /// <param name="_date">DateTime object holding forecast time</param>
        /// <param name="_saved">Represents if forecast card is saved by user</param>
        /// <param name="other8arguments">String array of 8 elements: Icon, Location, Temperature, Summary, 
        /// Humidity, Wind speed, Wind Direction, Water level</param>
        public ForecastCard(DateTime _date, bool _saved, string[] other8arguments)
        {
            date = _date;
            saved = _saved;
            if (other8arguments.Length >= 8)
            {
                icon = other8arguments[0];
                location = other8arguments[1];
                temperature = other8arguments[2];
                summary = other8arguments[3];
                humidity = other8arguments[4];
                windSpeed = other8arguments[5];
                windDirection = other8arguments[6];
                waterLevel = other8arguments[7];
            } else
                throw new Exception("To many string arguments!");            
        }

        /// <summary>
        /// Fetches DarkSky api, Tidal api to gather weather data, and select relevant data
        /// assigning it to this object
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <param name="location">Location name for provided coordinates</param>
        /// <param name="stationId">Tidal station id</param>
        /// <param name="addDays">Fetch data for future days. How many days add to today date</param>
        /// <param name="addHours">Fetch data for current hours + addHours</param>
        public void Update(double lat, double lon, string location, string stationId, int addDays = 0, int addHours = 0)
        {
            var client = new DarkSkyService(Constants.DarkSkyKey);
            // creating time object which depicts forecast time
            var time = DateTimeOffset.Now.AddDays(addDays).AddHours(addHours);
            // calling api for weather info
            DarkSkyApi.Models.Forecast f = Task.Run(() => client.GetTimeMachineWeatherAsync(lat, lon, time)).Result;
            // setting model properties with api result
            icon = f.Currently.Icon;
            this.location = location;
            double _temp = Math.Round(UnitConverters.FahrenheitToCelsius(f.Currently.Temperature), 1);
            temperature = _temp.ToString() + "°C";
            summary = f.Currently.Summary;
            humidity = $"{(int)(f.Currently.Humidity * 100)}%";
            windSpeed = string.Format("{0:0.00}m/s", f.Currently.WindSpeed / 2.237); // convert to m/s
            windDirection = ((int)(f.Currently.WindBearing)).ToString() + "°";
            if (stationId != string.Empty)
                waterLevel = TidalApi.GetInstance().GetWaterInfo(stationId, addDays, time.Hour);
            else
                waterLevel = "0m";
            // setting forecast time in model, setting minutes and seconds to 0
            date = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }
    }
}