using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("cities")]
    public IActionResult GetCities()
    {
        var cities = _weatherService.GetAvailableCities();
        return Ok(cities);
    }

    [HttpGet("{city}")]
    public IActionResult GetWeather(string city)
    {
        return Ok(new
        {
            message = $"Backend received city: {city}"
        });
    }
}