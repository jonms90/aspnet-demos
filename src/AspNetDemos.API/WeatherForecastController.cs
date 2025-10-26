using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AspNetDemos.API;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = 
        ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult<WeatherForecast[]> Get()
    {
        WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();
        return forecast;
    }

}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
