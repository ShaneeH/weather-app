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
    public async Task<IActionResult> GetWeather(string city)
    {
        if (!_weatherService.IsSupportedCity(city))
        {
            return BadRequest(new
            {
                error = "Invalid city.",
                allowedCities = _weatherService.GetAvailableCities()
            });
        }

        var weather = await _weatherService.GetWeatherByCityAsync(city);

        return Ok(weather);
    }
}