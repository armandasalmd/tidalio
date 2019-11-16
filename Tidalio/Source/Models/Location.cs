using System;

namespace Tidalio
{
    /// <summary>
    /// A model to hold data about particular location associated with forecast card
    /// </summary>
    public class Location
    {
        private int id;
        private string address;
        private double longitude, latitude;
        
        /// <summary>
        /// Initialized model, autofills coordinates according to the address
        /// </summary>
        /// <param name="address">Address of location you want to refer</param>
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
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Initialized model using coordinated. Address get automatically filled in using coordinates
        /// </summary>
        /// <param name="latitude">Location coordinate</param>
        /// <param name="longitude">Location coordinate</param>
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
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Initialized model with provided values
        /// </summary>
        /// <param name="location">Address of location</param>
        /// <param name="latitude">Location coordinate</param>
        /// <param name="longitude">Location coordinate</param>
        public Location(string location, double latitude, double longitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
            address = location;
        }

        /// <summary>
        /// Checks if values are initialized
        /// </summary>
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