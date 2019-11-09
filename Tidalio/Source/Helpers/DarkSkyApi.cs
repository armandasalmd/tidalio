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
using Xamarin.Android.Net;

namespace Tidalio
{
    public class DarkSkyApi
    {
        private static DarkSkyApi instance;
        private static HttpClient client;
        private DarkSkyApi()
        {
            client = new HttpClient(new AndroidClientHandler());
        }
        public static DarkSkyApi GetInstance()
        {
            if (instance == null)
                instance = new DarkSkyApi();
            return instance;
        }
        // Code starts here

    }
}