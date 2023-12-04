using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Models
{
    public class WeatherInfo
    {
        public DateTime Date { get; }

        public WeatherType Type { get; }
        
        public double Temperature { get; }

        public double Rain { get; }

        public WeatherInfo(DateTime date, WeatherType type, double temperature, double rain)
        {
            Date = date;
            Type = type;
            Temperature = temperature;
            Rain = rain;
        }
    }

    public static class WeatherInfoExt
    {
        public static WeatherInfo ToWeatherInfo(this WeatherDaily weatherDaily)
            => new WeatherInfo(weatherDaily.Date, weatherDaily.Type, weatherDaily.Temperature, weatherDaily.Rain);

        public static List<WeatherInfo> ToWeatherInfoList(this List<WeatherDaily> weatherDailies)
            => weatherDailies
                .Select(w => new WeatherInfo(w.Date, w.Type, w.Temperature, w.Rain))
                .ToList();
    }
}