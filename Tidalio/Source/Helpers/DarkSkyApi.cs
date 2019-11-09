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
    public class DarkSkyApi
    {
        private static DarkSkyApi instance;
        private DarkSkyApi()
        {
            return;
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