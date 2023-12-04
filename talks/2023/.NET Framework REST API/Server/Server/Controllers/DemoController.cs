using Server.Domain;
using Server.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class DemoController : Controller
    {
        async Task<WeatherDemo> GetCountryWeather() 
            => new WeatherDemo()
                {
                    WeatherData = (await new WeatherDailyRepository(new WeatherDbContext()).Country7Days(DateTime.UtcNow)).ToWeatherInfoList(),
                };

        public async Task<ActionResult> MVC()
        {
            return View(await GetCountryWeather());
        }

        public async Task<ActionResult> WebApp()
        {
            return View(await GetCountryWeather());
        }

        public async Task<ActionResult> WebForm()
        {
            return View(await GetCountryWeather());
        }

        public async Task<ActionResult> SPA()
        {
            return View(await GetCountryWeather());
        }

        public async Task<ActionResult> Svelte()
        {
            return View(await GetCountryWeather());
        }
        public async Task<ActionResult> Astro()
        {
            return View(await GetCountryWeather());
        }
    }
}