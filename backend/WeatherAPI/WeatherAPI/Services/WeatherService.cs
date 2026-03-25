using WeatherAPI.Clients.Interfaces;
using WeatherAPI.Dtos;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Services;

public class WeatherService : IWeatherService
{
    // HashSet for Peformance faster than a List and since all entries are unique
    private static readonly HashSet<string> _availableCities = new(StringComparer.OrdinalIgnoreCase)
    {
        "Dublin",
        "Tokyo",
        "Chicago"
    };

    private readonly IWeatherApiClient _weatherApiClient;

    public WeatherService(IWeatherApiClient weatherApiClient)
    {
        _weatherApiClient = weatherApiClient;

    }

    public IReadOnlyCollection<string> GetAvailableCities()
    {
        return _availableCities; ;
    }

    public bool IsSupportedCity(string city)
    {
        return !string.IsNullOrWhiteSpace(city) && _availableCities.Contains(city);
    }

    public async Task<WeatherResponseDto> GetWeatherByCityAsync(string city)
    {
        var normalizedCity = GetNormalizedCity(city);

        var currentWeatherTask = _weatherApiClient.GetCurrentWeatherAsync(normalizedCity);
        var timezoneTask = _weatherApiClient.GetTimezoneAsync(normalizedCity);
        var astronomyTask = _weatherApiClient.GetAstronomyAsync(normalizedCity);

        // Run the three API calls concurrently to reduce response time
        await Task.WhenAll(currentWeatherTask, timezoneTask, astronomyTask);

        var currentWeather = await currentWeatherTask;
        var timezone = await timezoneTask;
        var astronomy = await astronomyTask;

        // This is the Final Aggregated Object we Return to the CLient
        return new WeatherResponseDto
        {
            City = normalizedCity,
            Weather = new WeatherDetailsDto
            {
                TemperatureC = currentWeather.Current.TemperatureC,
                Condition = currentWeather.Current.Condition.Text,
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

    private static string GetNormalizedCity(string city)
    {
        return _availableCities.First(c => c.Equals(city, StringComparison.OrdinalIgnoreCase));
    }
}