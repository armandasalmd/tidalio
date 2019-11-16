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

namespace Tidalio.Newton
{
    public class ForecastCardNewton
    {
        public string user_email { get; set; }
        public string forecast_date { get; set; }
        public string icon { get; set; }
        public string forecast_location { get; set; }
        public string temperature { get; set; }
        public string summary { get; set; }
        public string humidity { get; set; }
        public string wind_speed { get; set; }
        public string wind_direction { get; set; }
        public string water_level { get; set; }
    }
}