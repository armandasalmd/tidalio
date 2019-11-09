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
            get { return date.ToString("dd/MM/yyyy hh:mm"); }
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
        public string WaterLevel { get { return waterLevel; } }

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
    }
}