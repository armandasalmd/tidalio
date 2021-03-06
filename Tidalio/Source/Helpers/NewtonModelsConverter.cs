﻿using System.Collections.Generic;

namespace Tidalio
{
    /// <summary>
    /// Converts 2 different type models: JSON model, to Regular model
    /// </summary>
    public static class NewtonModelsConverter
    {
        /// <summary>
        /// Converts ForecastCard model to JSON model
        /// </summary>
        /// <param name="user_email">User email</param>
        /// <param name="f">Forecast card model instance</param>
        /// <returns>Newton model</returns>
        public static Newton.ForecastCardNewton ForecastCard_To_ForecastCardNewton(string user_email, ForecastCard f)
        {
            Newton.ForecastCardNewton result = new Newton.ForecastCardNewton();
            result.user_email = user_email;
            result.forecast_date = f.DateFormated;
            result.icon = f.Icon;
            result.forecast_location = f.Location;
            result.temperature = f.Temperature;
            result.summary = f.Summary;
            result.humidity = f.Humidity;
            result.wind_speed = f.WindSpeed;
            result.wind_direction = f.WindDirection;
            result.water_level = f.WaterLevel;
            return result;
        }

        /// <summary>
        /// Converts Location model to JSON model
        /// </summary>
        /// <param name="user_email">User email</param>
        /// <param name="loc">Location model instance</param>
        /// <returns>Newton model</returns>
        public static Newton.LocationNewton Location_To_LocationNewton(string user_email, Location loc)
        {
            Newton.LocationNewton result = new Newton.LocationNewton();
            result.user_email = user_email;
            result.title = loc.Address;
            result.latitude = loc.Latitude;
            result.longitude = loc.Longitude;
            return result;
        }

        /// <summary>
        /// Converts JSON model to list of TidalStation models
        /// </summary>
        /// <param name="data">Instance to convert from</param>
        /// <returns>List of TidalStation's</returns>
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

        /// <summary>
        /// Converts JSON model to list of TidalEvent model
        /// </summary>
        /// <param name="dataList">Data to convert from</param>
        /// <param name="stationId">Reference to station</param>
        /// <returns>List of TidalEvent's</returns>
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

        /// <summary>
        /// Convert JSON model list into List of Location model
        /// </summary>
        /// <param name="dataList">Data to convert from</param>
        /// <returns>List of Location's</returns>
        public static List<Location> ListLocationNewton_ToList_Location(List<Newton.LocationNewton> dataList)
        {
            List<Location> result = new List<Location>();
            if (dataList != null)
            {
                foreach (Newton.LocationNewton e in dataList)
                {
                    result.Add(new Location(e.title, e.latitude, e.longitude));
                }
            }
            return result;
        }

        /// <summary>
        /// Convert JSON model list into List of ForecastCard model
        /// </summary>
        /// <param name="dataList">Data to convert from</param>
        /// <returns>List of ForecastCard's</returns>
        public static List<ForecastCard> ListForecasCardtNewton_ToList_ForecastCard(List<Newton.ForecastCardNewton> dataList)
        {
            List<ForecastCard> result = new List<ForecastCard>();
            if (dataList != null)
            {
                foreach (Newton.ForecastCardNewton e in dataList)
                {
                    result.Add(new ForecastCard(e));
                }
            }
            return result;
        }
    }
}