using WeatherAPI.Clients.Interfaces;
using WeatherAPI.Dtos;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Services;

public class WeatherService : IWeatherService
{
    private static readonly HashSet<string> AvailableCities = new(StringComparer.OrdinalIgnoreCase)
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
        return AvailableCities.ToList().AsReadOnly();
    }

    public bool IsSupportedCity(string city)
    {
        return !string.IsNullOrWhiteSpace(city) && AvailableCities.Contains(city);
    }

    public async Task<WeatherResponseDto> GetWeatherByCityAsync(string city)
    {
        var normalizedCity = GetNormalizedCity(city);

        var currentWeatherTask = _weatherApiClient.GetCurrentWeatherAsync(normalizedCity);
        var timezoneTask = _weatherApiClient.GetTimezoneAsync(normalizedCity);
        var astronomyTask = _weatherApiClient.GetAstronomyAsync(normalizedCity);

        await Task.WhenAll(currentWeatherTask, timezoneTask, astronomyTask);

        var currentWeather = await currentWeatherTask;
        var timezone = await timezoneTask;
        var astronomy = await astronomyTask;

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
        if (string.Equals(city, "chicago", StringComparison.OrdinalIgnoreCase))
        {
            return "Chicago";
        }

        if (string.Equals(city, "tokyo", StringComparison.OrdinalIgnoreCase))
        {
            return "Tokyo";
        }

        return "Dublin";
    }
}