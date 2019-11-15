using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DarkSkyApi;
using Xamarin.Essentials;

namespace Tidalio
{
    public class ForecastCard
    {
        private DateTime date;
        private bool saved;
        private string icon, location, 
            temperature, summary, 
            humidity, windSpeed, 
            windDirection, waterLevel;

        public string DateFormated
        {
            get { return date.ToString("dd/MM/yyyy HH:mm"); }
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
        public string WaterLevel {
            get {
                if (waterLevel == "0m")
                    return "unavailable";
                else
                    return waterLevel;
            }
        }

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


        public void Update(double lat, double lon, string location, string stationId, int addDays = 0, int addHours = 0)
        {
            var client = new DarkSkyService(Constants.DarkSkyKey);
            var time = DateTimeOffset.Now.AddDays(addDays).AddHours(addHours);

            DarkSkyApi.Models.Forecast f = Task.Run(() => client.GetTimeMachineWeatherAsync(lat, lon, time)).Result;

            icon = f.Currently.Icon;
            this.location = location;
            double _temp = Math.Round(UnitConverters.FahrenheitToCelsius(f.Currently.Temperature), 1);
            temperature = _temp.ToString() + "°C";
            summary = f.Currently.Summary;
            humidity = $"{(int)(f.Currently.Humidity * 100)}%";
            windSpeed = string.Format("{0:0.00}m/s", f.Currently.WindSpeed / 2.237); // convert to m/s
            // windSpeed = Math.Round(, ).ToString() + "m/s"; 
            windDirection = ((int)(f.Currently.WindBearing)).ToString() + "°";

            // TODO: waterLevel
            if (stationId != string.Empty)
                waterLevel = TidalApi.GetInstance().GetWaterInfo(stationId, addDays, time.Hour);
            else
                waterLevel = "0m";
            date = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }

        public async void UpdateAsync(double lat, double lon)
        {
            var client = new DarkSkyService(Constants.DarkSkyKey);
            DarkSkyApi.Models.Forecast forecast = await client.GetTimeMachineWeatherAsync(lat, lon, DateTimeOffset.Now);
        }
    }
}