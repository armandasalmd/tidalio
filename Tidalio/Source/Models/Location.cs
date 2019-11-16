using System;

namespace Tidalio
{
    public class Location
    {
        private int id;
        private string address;
        private double longitude, latitude;
        public Location(string address)
        {
            this.address = address;
            try
            {
                double[] coordinates = Functions.CalculateCoordinates(address);
                latitude = coordinates[0];
                longitude = coordinates[1];
            } catch (Exception ex)
            {
                // Console.WriteLine(ex.message);
                throw ex;
            }
        }

        public Location(double latitude, double longitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
            try
            {
                address = Functions.CalculateAddress(latitude, longitude);
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.message);
                throw ex;
            }
        }

        public Location(string location, double latitude, double longitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
            address = location;
        }

        public bool IsValid
        {
            get { return address != null && longitude != 0 && latitude != 0; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public int Id { get { return id; } set { id = value; } }
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

    }
}