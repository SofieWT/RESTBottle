using Microsoft.AspNetCore.Mvc;

namespace RESTBottle.Controllers
{
    [ApiController]
    [Route("[controller]")] //Dette er en route, som bestemmer hvordan man kan tilgå controller og med kant paraenteser fortæller det at controller navnet skal bruges som route. Så hvis controlleren hedder WeatherForecastController, så bliver route til /WeatherForecast
    public class WeatherForecastController : ControllerBase //man skal extend ControllerBase for at kunne bruge Controller funktionalitet
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
