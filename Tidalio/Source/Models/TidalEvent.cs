using System;

namespace Tidalio
{
    /// <summary>
    /// A model to store tidal event data fetched from tidal API
    /// </summary>
    public class TidalEvent
    {
        private string stationId;
        private string dateTime;
        private double height;
        private string description;

        /// <summary>
        /// Initialize model with default values
        /// </summary>
        public TidalEvent()
        {
            dateTime = "2019-11-12T01:03:00";
            height = 0;
            description = "LowWater";
        }

        /// <summary>
        /// Initialize the model with provided values
        /// </summary>
        /// <param name="_dateTime">Tidal event time</param>
        /// <param name="_height">Water hight</param>
        /// <param name="_desc">tidal event description</param>
        /// <param name="sId">Station id that references tidal event</param>
        public TidalEvent(string _dateTime, double _height, string _desc, string sId)
        {
            dateTime = _dateTime;
            height = _height;
            description = _desc;
            stationId = sId;
        }

        public string StationId { get => stationId; set => stationId = value; }
        public string DateTime {
            get => dateTime.ToString();
            set {
                dateTime = value;
            }
        }

        /// <summary>
        /// Converts model string date int DateTime Object
        /// </summary>
        /// <returns>DateTime object of tidal event</returns>
        public DateTime GetDateTimeObj() {
            try
            {
                int y = int.Parse(dateTime.Substring(0, 4)), 
                    m = int.Parse(dateTime.Substring(5, 2)), 
                    d = int.Parse(dateTime.Substring(8, 2));
                int h = int.Parse(dateTime.Substring(11, 2)), 
                    mm = int.Parse(dateTime.Substring(14, 2)), 
                    s = int.Parse(dateTime.Substring(17, 2));
                return new System.DateTime(y, m, d, h, mm, s);
            } catch (Exception e)
            {
                return new DateTime();
            }            
        }
        public double Height { get => height; set => height = value; }
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// Prepares model to be display
        /// </summary>
        /// <returns>Returns string about tidal event</returns>
        public override string ToString()
        {
            return $"{description} {Math.Round(height, 2).ToString()}m";
        }
    }
}