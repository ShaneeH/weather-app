using System.Text.Json;
using Microsoft.Extensions.Options;
using WeatherAPI.Clients.Interfaces;
using WeatherAPI.Configuration;
using WeatherAPI.Dtos.External;

namespace WeatherAPI.Clients;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _weatherApiOptions;

    public WeatherApiClient(HttpClient httpClient, IOptions<WeatherApiOptions> weatherApiOptions)
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

    // Current Weather Endpoint
    public Task<CurrentWeatherApiResponseDto> GetCurrentWeatherAsync(string city) 
    {
        return GetFromApiAsync<CurrentWeatherApiResponseDto>($"current.json?q={city}");
    }

    // Timezone Endpoint
    public Task<TimezoneApiResponseDto> GetTimezoneAsync(string city)
    {
        return GetFromApiAsync<TimezoneApiResponseDto>($"timezone.json?q={city}");
    }

    // Astronomy Endpoint
    public Task<AstronomyApiResponseDto> GetAstronomyAsync(string city)
    {
        return GetFromApiAsync<AstronomyApiResponseDto>($"astronomy.json?q={city}");
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
}