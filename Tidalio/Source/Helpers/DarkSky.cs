using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DarkSkyApi;
using DarkSkyApi.Models;
using Xamarin.Android.Net;

namespace Tidalio
{
    public class DarkSky
    {
        private static DarkSky instance;
        private static HttpClient client;
        private DarkSky()
        {
            client = new HttpClient(new AndroidClientHandler());
        }
        public static DarkSky GetInstance()
        {
            if (instance == null)
                instance = new DarkSky();
            return instance;
        }

        public static DarkSkyService GetClient()
        {
            return new DarkSkyService(Constants.DarkSkyKey);
        }
        // Code starts here

    }
}