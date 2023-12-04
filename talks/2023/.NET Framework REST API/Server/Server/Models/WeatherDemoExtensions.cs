using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;

namespace Server.Models
{
    public static class WeatherDemoExtensions
    {
        static Dictionary<WeatherType, string> ConditionsIcons = new Dictionary<WeatherType, string>()
        {
            { WeatherType.Sunny, "fa-solid fa-sun" },
            { WeatherType.Cloudy, "fa-solid fa-cloud" },
            { WeatherType.Rainy, "fa-solid fa-cloud-rain" },
            { WeatherType.Windy, "fa-solid fa-wind" },
            { WeatherType.Stormy, "fa-solid fa-cloud-bolt" }
        };

        public static HtmlString ToHtml(this WeatherType weatherType)
        {
            var icon = ConditionsIcons[weatherType];
            return new HtmlString($"<i class=\"{icon}\"></i>");
        }
    }
}