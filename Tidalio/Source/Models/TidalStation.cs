namespace Tidalio
{
    public class TidalStation
    {
        private string id;
        private double lat, lon;
        private string name, country;

        public TidalStation()
        {
            id = "0000";
            lat = 0;
            lon = 0;
            name = "";
            country = "";
        }

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

        public override string ToString()
        {
            return $"{name}, {country}, {id}";
        }
    }
}