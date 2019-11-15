using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Tidalio
{
    public static class Functions
    {
        public static long GetUnixTimestamp()
        {
            DateTime timeNow = DateTime.UtcNow;
            return ((DateTimeOffset)timeNow).ToUnixTimeSeconds();
        }

        public static async Task<string> GetCoords(string address)
        {
            try
            {
                var locations = await Geocoding.GetLocationsAsync(address);
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "not found";
        }

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

        public static string GetDate(int addDays = 0)
        {
            DateTime d = DateTime.Now.AddDays(addDays);
            return $"{d.Day}/{d.Month}/{d.Year}";
        }
        public static int HoursDeltaToNow(string time)
        {
            string hours = time.Split(':')[0];
            try
            {
                return int.Parse(hours) - DateTimeOffset.Now.Hour;
            } catch (Exception ex)
            {
                return 0;
            }
        }

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

        //public static 

    }
}