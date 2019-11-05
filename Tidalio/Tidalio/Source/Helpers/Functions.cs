using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Tidalio.Source.Helpers
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
    }
}