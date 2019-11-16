namespace Tidalio
{
    /// <summary>
    /// Model for holding data about station fetched from tidal api (admirality)
    /// </summary>
    public class TidalStation
    {

        private string id;
        private double lat, lon;
        private string name, country;

        /// <summary>
        /// Initialize with 0 or "" property values
        /// </summary>
        public TidalStation()
        {
            id = "0000";
            lat = 0;
            lon = 0;
            name = "";
            country = "";
        }

        /// <summary>
        /// Initialize model with values
        /// </summary>
        /// <param name="_id">Tidal station API id</param>
        /// <param name="_lat">Station coordinate</param>
        /// <param name="_lon">Station coordinate</param>
        /// <param name="_name">Station name, expressed in location name</param>
        /// <param name="_country">Country in which station exists</param>
        public TidalStation(string _id, double _lat, double _lon, string _name, string _country)
        {
            id = _id;
            lat = _lat;
            lon = _lon;
            name = Functions.NormalizeString(_name);
            country = Functions.NormalizeString(_country);
        }

        public string Id { get => id; set => id = value; }
        public double Lat { get => lat; set => lat = value; }
        public double Lon { get => lon; set => lon = value; }
        public string Name { get => name; set => name = value; }
        public string Country { get => country; set => country = value; }

        /// <summary>
        /// Concatinates some object properties to form a display string
        /// </summary>
        /// <returns>Ready to display information about station</returns>
        public override string ToString()
        {
            return $"{name}, {country}, {id}";
        }
    }
}