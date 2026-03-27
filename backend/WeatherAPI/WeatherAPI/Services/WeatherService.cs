using WeatherAPI.Clients.Interfaces;
using WeatherAPI.Dtos;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Services;

public class WeatherService : IWeatherService
{
    // HashSet for  performance faster than List since all entries are unique
    private static readonly HashSet<string> AvailableCities = new(StringComparer.OrdinalIgnoreCase)
    {
        "Dublin",
        "Tokyo",
        "Chicago"
    };

    private readonly IWeatherApiClient _weatherApiClient;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(IWeatherApiClient weatherApiClient, ILogger<WeatherService> logger)
    {
        _weatherApiClient = weatherApiClient;
        _logger = logger;
    }

    public IReadOnlyCollection<string> GetAvailableCities() => AvailableCities;

    public bool IsSupportedCity(string city) =>
        !string.IsNullOrWhiteSpace(city) && AvailableCities.Contains(city);

    public async Task<WeatherResponseDto> GetWeatherByCityAsync(string city)
    {
        var normalizedCity = GetNormalizedCity(city);

        _logger.LogInformation("Fetching weather data for city: {City}", normalizedCity);

        
        var currentWeatherTask = _weatherApiClient.GetCurrentWeatherAsync(normalizedCity);
        var timezoneTask = _weatherApiClient.GetTimezoneAsync(normalizedCity);
        var astronomyTask = _weatherApiClient.GetAstronomyAsync(normalizedCity);

        try
        {
            // Fire all three API calls concurrently to reduce total response time

            await Task.WhenAll(currentWeatherTask, timezoneTask, astronomyTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "One or more WeatherAPI calls failed for city: {City}", normalizedCity);
            throw;
        }

        var currentWeather = currentWeatherTask.Result;
        var timezone = timezoneTask.Result;
        var astronomy = astronomyTask.Result;

        _logger.LogInformation("Successfully fetched weather data for city: {City}", normalizedCity);

        return new WeatherResponseDto
        {
            City = normalizedCity,

            Weather = new WeatherDetailsDto
            {
                TemperatureC = currentWeather.Current.TemperatureC,
                Condition = currentWeather.Current.Condition.Text,
                Icon = currentWeather.Current.Condition.Icon,
                WindKph = currentWeather.Current.WindKph,
                Humidity = currentWeather.Current.Humidity,
                Cloud = currentWeather.Current.Cloud,
                IsDay = currentWeather.Current.IsDay == 1
            },

            Timezone = new TimezoneDetailsDto
            {
                TimezoneId = timezone.Location.TimezoneId,
                LocalTime = timezone.Location.LocalTime,
                LocaltimeEpoch = timezone.Location.LocaltimeEpoch
            },

            Astronomy = new AstronomyDetailsDto
            {
                Sunrise = astronomy.Astronomy.Astro.Sunrise,
                Sunset = astronomy.Astronomy.Astro.Sunset,
                Moonrise = astronomy.Astronomy.Astro.Moonrise,
                Moonset = astronomy.Astronomy.Astro.Moonset,
                MoonPhase = astronomy.Astronomy.Astro.MoonPhase
            }
        };
    }

    // Normalises city casing — ensures "dublin" becomes "Dublin" before hitting the external API
    private static string GetNormalizedCity(string city) =>
        AvailableCities.First(c => c.Equals(city, StringComparison.OrdinalIgnoreCase));
}