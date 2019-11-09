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
    public class TidalStation
    {
        public int id;
        public double lat, lon;
        string name, country;

        public TidalStation()
        {
            id = 0;
            lat = 0;
            lon = 0;
            name = "";
            country = "";
        }
    }
}