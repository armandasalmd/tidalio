using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Tidalio
{
    public static class Functions
    {
        /// <summary>
        /// Converts coordinates into address or location
        /// </summary>
        /// <param name="_latitude">Latitude</param>
        /// <param name="_longitude">Longitude</param>
        /// <returns>Address for given coordinates</returns>
        public static string CalculateAddress(double _latitude, double _longitude)
        {
            var address = "";
            try
            {
                var placemarks = Task.Run(() => Geocoding.GetPlacemarksAsync(_latitude, _longitude)).Result;

                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    if (placemark.CountryCode != null)
                        address = $"{placemark.Thoroughfare},{placemark.PostalCode},{placemark.CountryName}";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return address;
        }

        /// <summary>
        /// Generates date string
        /// </summary>
        /// <param name="addDays">Days count to add to today date</param>
        /// <returns>Formated day/month/year string</returns>
        public static string GetDate(int addDays = 0)
        {
            DateTime d = DateTime.Now.AddDays(addDays);
            return $"{d.Day}/{d.Month}/{d.Year}";
        }

        /// <summary>
        /// Subtracts current hour with given HH:mm string hours
        /// </summary>
        /// <param name="time">Number of hours to subtract</param>
        /// <returns>Number representing hours</returns>
        public static int HoursDeltaToNow(string time)
        {
            string hours = time.Split(':')[0];
            try
            {
                return int.Parse(hours) - DateTimeOffset.Now.Hour;
            } catch (Exception)
            {
                // if hours is not a number
                return 0;
            }
        }

        /// <summary>
        /// Calculates coordinates for given address
        /// </summary>
        /// <param name="addr">Address</param>
        /// <returns>Coordinates in double[2] - longitude, latitude</returns>
        public static double[] CalculateCoordinates(string addr)
        {
            double[] coords = new double[2];
            try
            {
                var locations = Task.Run(() => Geocoding.GetLocationsAsync(addr)).Result;
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    coords[0] = location.Longitude;
                    coords[1] = location.Latitude;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return coords;
        }

        /// <summary>
        /// Returns drawable(image) resourse id
        /// </summary>
        /// <param name="icon">String description of icon</param>
        /// <returns>Drawable Resourse Id</returns>
        public static int GetIconDrawable(string icon)
        {
            int resId = Resource.Drawable.clear_day;
            switch (icon)
            {
                case "fog": resId = Resource.Drawable.fog; break;
                case "partly-cloudy-night": resId = Resource.Drawable.partly_cloudy_night; break;
                case "partly-cloudy-day": resId = Resource.Drawable.partly_cloudy_day; break;
                case "clear-day": resId = Resource.Drawable.clear_day; break;
                case "clear-night": resId = Resource.Drawable.clear_night; break;
                case "rain": resId = Resource.Drawable.rain; break;
                case "snow": resId = Resource.Drawable.snow; break;
                case "sleet": resId = Resource.Drawable.sleet; break;
                case "wind": resId = Resource.Drawable.wind; break;
                case "cloudy": resId = Resource.Drawable.cloud; break;
            }
            return resId;
        }

        /// <summary>
        /// Removes spaces, single capital letter per word
        /// </summary>
        /// <param name="input">String to normalize</param>
        /// <returns>Normalized string</returns>
        public static string NormalizeString(string input)
        {
            // Makes every word first letter capital, others lower, removes spaces
            string result = string.Empty;
            try
            {
                string w = "";
                string[] words = input.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    w = words[i];
                    result += w?.First().ToString().ToUpper() + w?.Substring(1).ToLower();
                    if (i != words.Length - 1) // if it is not the last loop then add space
                        result += " ";

                }
            } catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Replaces / with URL escape char %2F
        /// </summary>
        /// <param name="data">String to escape</param>
        /// <returns>Escape ready string</returns>
        public static string EscapeBackslash(string data)
        {
            return data.Replace("/", "%2F");
        }

    }
}