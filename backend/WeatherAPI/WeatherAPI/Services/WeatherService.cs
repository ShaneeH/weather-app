using Microsoft.Extensions.Options;
using WeatherAPI.Configuration;
using WeatherAPI.Dtos;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Services;

public class WeatherService : IWeatherService
{
    private static readonly HashSet<string> AvailableCities =
    [
        "Dublin",
        "Tokyo",
        "Chicago"
    ];

    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _weatherApiOptions;

    public WeatherService(HttpClient httpClient, IOptions<WeatherApiOptions> weatherApiOptions)
    {
        _httpClient = httpClient;
        _weatherApiOptions = weatherApiOptions.Value;
    }

    public IReadOnlyCollection<string> GetAvailableCities()
    {
        return AvailableCities.ToList().AsReadOnly();
    }

    public bool IsSupportedCity(string city)
    {
        return !string.IsNullOrWhiteSpace(city) &&
               AvailableCities.Contains(city);
    }

    public Task<WeatherResponseDto> GetWeatherByCityAsync(string city)
    {
        var response = new WeatherResponseDto
        {
            City = city,
            Weather = new WeatherDetailsDto
            {
                TemperatureC = 0,
                Condition = "Stub response",
                WindKph = 0,
                Humidity = 0,
                Cloud = 0,
                IsDay = true
            },
            Timezone = new TimezoneDetailsDto
            {
                TimezoneId = "Stub/Timezone",
                LocalTime = "Not loaded yet",
                LocaltimeEpoch = 0
            },
            Astronomy = new AstronomyDetailsDto
            {
                Sunrise = "Not loaded yet",
                Sunset = "Not loaded yet",
                Moonrise = "Not loaded yet",
                Moonset = "Not loaded yet",
                MoonPhase = "Not loaded yet"
            }
        };

        return Task.FromResult(response);
    }
}