using System.Text.Json;
using Microsoft.Extensions.Options;
using WeatherAPI.Clients.Interfaces;
using WeatherAPI.Configuration;
using WeatherAPI.Dtos.External;

namespace WeatherAPI.Clients;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherApiClient> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public WeatherApiClient(
        HttpClient httpClient,
        IOptions<WeatherApiOptions> weatherApiOptions,
        ILogger<WeatherApiClient> logger)
    {
        _logger = logger;

        var options = weatherApiOptions.Value;

        // Validate config at startup — fail fast rather than getting cryptic errors at runtime
        if (string.IsNullOrWhiteSpace(options.BaseUrl))
            throw new InvalidOperationException("WeatherApiOptions.BaseUrl is not configured.");

        if (string.IsNullOrWhiteSpace(options.ApiKey))
            throw new InvalidOperationException("WeatherApiOptions.ApiKey is not configured.");

        if (string.IsNullOrWhiteSpace(options.Host))
            throw new InvalidOperationException("WeatherApiOptions.Host is not configured.");

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.BaseUrl);

        // Only add headers once — guards against double-registration if the client is reused
        if (!_httpClient.DefaultRequestHeaders.Contains("X-RapidAPI-Key"))
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", options.ApiKey);

        if (!_httpClient.DefaultRequestHeaders.Contains("X-RapidAPI-Host"))
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", options.Host);
    }

    public Task<CurrentWeatherApiResponseDto> GetCurrentWeatherAsync(string city)
        => GetFromApiAsync<CurrentWeatherApiResponseDto>($"current.json?q={Uri.EscapeDataString(city)}");

    public Task<TimezoneApiResponseDto> GetTimezoneAsync(string city)
        => GetFromApiAsync<TimezoneApiResponseDto>($"timezone.json?q={Uri.EscapeDataString(city)}");

    public Task<AstronomyApiResponseDto> GetAstronomyAsync(string city)
        => GetFromApiAsync<AstronomyApiResponseDto>($"astronomy.json?q={Uri.EscapeDataString(city)}");

    // ── Private ───────────────────────────────────────────────────────────────

    private async Task<T> GetFromApiAsync<T>(string endpoint)
    {
        _logger.LogInformation("Calling WeatherAPI endpoint: {Endpoint}", endpoint);

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.GetAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error calling WeatherAPI endpoint: {Endpoint}", endpoint);
            throw new HttpRequestException(
                $"Network error while contacting WeatherAPI for endpoint: {endpoint}", ex);
        }
        catch (TaskCanceledException ex)
        {
            // Covers both timeout and genuine cancellation
            _logger.LogError(ex, "Request timed out calling WeatherAPI endpoint: {Endpoint}", endpoint);
            throw new TimeoutException(
                $"Request to WeatherAPI timed out for endpoint: {endpoint}", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            _logger.LogWarning(
                "WeatherAPI returned {StatusCode} for endpoint: {Endpoint}. Body: {Body}",
                (int)response.StatusCode, endpoint, body);

            throw new HttpRequestException(
                $"WeatherAPI returned {(int)response.StatusCode} for endpoint: {endpoint}");
        }

        var json = await response.Content.ReadAsStringAsync();

        T? result;

        try
        {
            result = JsonSerializer.Deserialize<T>(json, JsonOptions);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize WeatherAPI response for endpoint: {Endpoint}", endpoint);
            throw new InvalidOperationException(
                $"Failed to deserialize WeatherAPI response for endpoint: {endpoint}", ex);
        }

        if (result is null)
        {
            _logger.LogError("Deserialization returned null for endpoint: {Endpoint}", endpoint);
            throw new InvalidOperationException(
                $"WeatherAPI response deserialized to null for endpoint: {endpoint}");
        }

        return result;
    }
}