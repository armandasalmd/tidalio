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
    public class TidalEventNewton
    {
        public string EventType { get; set; }
        public string DateTime { get; set; }
        public bool IsApproximateTime { get; set; }
        public double Height { get; set; }
        public bool IsApproximateHeight { get; set; }
        public bool Filtered { get; set; }


    }

}