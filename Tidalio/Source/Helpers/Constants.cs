﻿using System;

namespace Tidalio
{
    /// <summary>
    /// Contains api keys, domains to apis...
    /// </summary>
    public static class Constants
    {
        // Firebase auth variables
        public const string AuthApplicationId = "1:985581754028:android:95b71532953e2cf8fff109";
        public const string AuthApiKey = "AIzaSyAlASiFw6wz0lvAjhwMzsfH_rWAu5ycIAM";
        public const string AuthDatabaseUrl = "https://tidalioauth-8377f.firebaseio.com";
        public const string AuthProjectId = "tidalioauth-8377f";
        public const string AuthStorageBucket = "tidalioauth-8377f.appspot.com";
        public const string Sha1Signature = "AB:60:A1:C5:27:25:48:61:7F:2A:93:0F:A4:B0:20:DA:75:2E:EF:AA";

        // API keys
        public const string DarkSkyKey = "240bb2915e752c9dcf6e0cc03eb58b95";
        public const string TidalKey   = "ddf7e05ca7fb49cb930cf28e9291504f";

        // API domains
        public const string DarkSkyDomain = "https://api.darksky.net";
        public const string TidalDomain   = "https://admiraltyapi.azure-api.net";
        public const string TidalioDomain = "https://tidalioapi.armandasbark.now.sh";

        /// <summary>
        /// Generates sample forecast card model instance
        /// </summary>
        /// <returns>Sample ForecastCard</returns>
        public static ForecastCard GetSampleForecastCard()
        {
            string[] array = { "wind", "Coventry, United Kingdom", "16°C", "Rain till evening", "75%", "5.5m/s", "225°", "4.22m" };
            return new ForecastCard(DateTime.Now, true, array);
        }

        // Autocomplete component tutorial: https://www.youtube.com/watch?v=JB3ETK5mh3c
    }
}