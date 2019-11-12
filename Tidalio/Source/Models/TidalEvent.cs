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
    public class TidalEvent
    {
        private string stationId;
        private string dateTime;
        private double height;
        private string description;

        public TidalEvent()
        {
            dateTime = "2019-11-12T01:03:00";
            height = 0;
            description = "LowWater";
        }

        public TidalEvent(string _dateTime, double _height, string _desc, string sId)
        {
            dateTime = _dateTime;
            height = _height;
            description = _desc;
            stationId = sId;
        }

        public string StationId { get => stationId; set => stationId = value; }
        public string DateTime { get => dateTime; set => dateTime = value; }
        public double Height { get => height; set => height = value; }
        public string Description { get => description; set => description = value; }

        public override string ToString()
        {
            return $"{description} {Math.Round(height, 2).ToString()}m";
        }
    }
}