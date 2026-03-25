using System.Text.Json;
using Microsoft.Extensions.Options;
using WeatherAPI.Configuration;
using WeatherAPI.Dtos;
using WeatherAPI.Dtos.External;
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

        if (!string.IsNullOrWhiteSpace(_weatherApiOptions.BaseUrl))
        {
            _httpClient.BaseAddress = new Uri(_weatherApiOptions.BaseUrl);
        }

        if (!_httpClient.DefaultRequestHeaders.Contains("X-RapidAPI-Key"))
        {
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _weatherApiOptions.ApiKey);
        }

        if (!_httpClient.DefaultRequestHeaders.Contains("X-RapidAPI-Host"))
        {
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _weatherApiOptions.Host);
        }
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

    public async Task<WeatherResponseDto> GetWeatherByCityAsync(string city)
    {
        var normalizedCity = GetNormalizedCity(city);

        var currentWeatherTask = GetFromApiAsync<CurrentWeatherApiResponseDto>($"current.json?q={normalizedCity}");
        var timezoneTask = GetFromApiAsync<TimezoneApiResponseDto>($"timezone.json?q={normalizedCity}");
        var astronomyTask = GetFromApiAsync<AstronomyApiResponseDto>($"astronomy.json?q={normalizedCity}");

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

    private async Task<T> GetFromApiAsync<T>(string endpoint)
    {
        using var response = await _httpClient.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<T>(json);

        if (result is null)
        {
            throw new InvalidOperationException($"Failed to deserialize response for endpoint: {endpoint}");
        }

        return result;
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