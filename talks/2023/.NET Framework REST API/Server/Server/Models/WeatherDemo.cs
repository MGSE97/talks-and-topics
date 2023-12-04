using System.Collections.Generic;

namespace Server.Models
{
    public class WeatherDemo
    {
        public string Search { get; set; }
        public List<WeatherInfo> WeatherData { get; set; }

        public WeatherDemo()
        {
        }

        public WeatherDemo(string search, List<WeatherInfo> weatherData)
        {
            Search = search;
            WeatherData = weatherData;
        }
    }
}