using Server.Domain;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Server.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeatherController : ApiController
    {
        private WeatherDailyRepository _weatherDailyRepository;

        public WeatherController()
        {
            _weatherDailyRepository = new WeatherDailyRepository(new WeatherDbContext());
        }

        // GET api/weather
        // Gets weather for whole country
        public async Task<List<WeatherInfo>> Get() 
            => (await _weatherDailyRepository.Country7Days(DateTime.UtcNow))
                    .ToWeatherInfoList();

        // GET api/weather/locations/{location}
        // Gets weather for specific location
        [Route("locations/{location}")]
        public async Task<List<WeatherInfo>> Get(string location)
            => (await _weatherDailyRepository.Search7Days(DateTime.UtcNow, location))
                    .ToWeatherInfoList();

        // POST api/weather/locations/search
        // Search locations
        [Route("locations/search")]
        public async Task<List<string>> Post([FromBody] string value)
            => await _weatherDailyRepository.GetLocations(value);
    }
}