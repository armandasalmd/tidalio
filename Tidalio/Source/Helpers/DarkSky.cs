using System.Net.Http;
using DarkSkyApi;
using Xamarin.Android.Net;

namespace Tidalio
{
    public class DarkSky
    {
        private static DarkSky instance;
        private static HttpClient client;
        private DarkSky()
        {
            client = new HttpClient(new AndroidClientHandler());
        }
        public static DarkSky GetInstance()
        {
            if (instance == null)
                instance = new DarkSky();
            return instance;
        }

        public static DarkSkyService GetClient()
        {
            return new DarkSkyService(Constants.DarkSkyKey);
        }
        // Code starts here

    }
}