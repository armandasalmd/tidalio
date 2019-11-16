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
    public class LocationNewton
    {
        public string user_email { get; set; }
        public string title { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}