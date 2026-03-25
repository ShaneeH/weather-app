using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    [HttpGet("{city}")]
    public IActionResult GetWeatherByCity(string city)
    {
        return Ok(new
        {
            message = $"Backend received city: {city}"
        });
    }
}