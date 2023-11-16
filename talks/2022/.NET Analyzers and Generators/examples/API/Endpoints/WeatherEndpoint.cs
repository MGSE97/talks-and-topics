using Microsoft.AspNetCore.Components;

namespace API.Endpoints
{
    [Route("/weather")]
    public class WeatherEndpoint
    {
        static string[] SUMMARIES = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecast[] Get()
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                  new WeatherForecast
                  (
                      DateTime.Now.AddDays(index),
                      Random.Shared.Next(-20, 55),
                      SUMMARIES[Random.Shared.Next(SUMMARIES.Length)]
                  ))
                   .ToArray();
            return forecast;
        }
    }

    public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
