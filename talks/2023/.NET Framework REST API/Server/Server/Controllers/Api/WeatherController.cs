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

        /// <summary>
        /// Gets weather for whole country
        /// </summary>
        /// <returns>Country weather for next 7 days</returns>
        // GET api/weather
        public async Task<List<WeatherInfo>> Get() 
            => (await _weatherDailyRepository.Country7Days(DateTime.UtcNow))
                    .ToWeatherInfoList();

        /// <summary>
        /// Gets weather for specific location
        /// </summary>
        /// <returns>Location weather for next 7 days</returns>
        // GET api/weather/locations/{location}
        [Route("locations/{location}")]
        public async Task<List<WeatherInfo>> Get(string location)
            => (await _weatherDailyRepository.Search7Days(DateTime.UtcNow, location))
                    .ToWeatherInfoList();

        /// <summary>
        /// Search locations for autocomplete
        /// </summary>
        /// <returns>Valid locations</returns>
        // POST api/weather/locations/search
        [Route("locations/search")]
        public async Task<List<string>> Post([FromBody] string value)
            => await _weatherDailyRepository.GetLocations(value);
    }
}