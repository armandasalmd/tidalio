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
    public static class NewtonModelsConverter
    {

        public static List<TidalStation> TidalStationsNewton_ToList_TidalStation(Newton.TidalStationsNewton data)
        {
            List<TidalStation> result = new List<TidalStation>();
            if (data != null)
            {
                foreach (Newton.Feature f in data.features)
                {
                    result.Add(new TidalStation(f.properties.Id, f.geometry.coordinates[1],
                        f.geometry.coordinates[0], f.properties.Name, f.properties.Country));
                }

            }

            return result;
        }

        public static List<TidalEvent> TidalEventNewton_ToList_TidalEvent(List<Newton.TidalEventNewton> dataList, string stationId)
        {
            List<TidalEvent> result = new List<TidalEvent>();
            if (dataList != null)
            {
                foreach (Newton.TidalEventNewton e in dataList)
                {
                    result.Add(new TidalEvent(e.DateTime, e.Height, e.EventType, stationId));
                }
            }
            return result;
        }


    }
}