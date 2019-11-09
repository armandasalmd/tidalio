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
    public static class Constants
    {
        // Public references
        public const string AuthApplicationId = "1:985581754028:android:95b71532953e2cf8fff109";
        public const string AuthApiKey = "AIzaSyAlASiFw6wz0lvAjhwMzsfH_rWAu5ycIAM";
        public const string AuthDatabaseUrl = "https://tidalioauth-8377f.firebaseio.com";
        public const string AuthProjectId = "tidalioauth-8377f";
        public const string AuthStorageBucket = "tidalioauth-8377f.appspot.com";
        public const string Sha1Signature = "AB:60:A1:C5:27:25:48:61:7F:2A:93:0F:A4:B0:20:DA:75:2E:EF:AA";

        // Autocomplete component tutorial: https://www.youtube.com/watch?v=JB3ETK5mh3c
        public const string DarkSkyKey = "240bb2915e752c9dcf6e0cc03eb58b95";
        public const string TidalKey   = "ddf7e05ca7fb49cb930cf28e9291504f";

        public const string DarkSkyDomain = "https://api.darksky.net";
        public const string TidalDomain   = "https://admiraltyapi.azure-api.net";

        public static ForecastCard GetSampleForecastCard()
        {
            // alt 0176 = °
            string[] array = { "wind", "Coventry, United Kingdom", "16°C", "Rain till evening", "75%", "5.5m/s", "225°", "4.22m" };
            return new ForecastCard(DateTime.Now, true, array);
        }
    }
}