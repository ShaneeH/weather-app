using WeatherAPI.Dtos.External;

namespace WeatherAPI.Clients.Interfaces;

public interface IWeatherApiClient
{
    Task<CurrentWeatherApiResponseDto> GetCurrentWeatherAsync(string city);
    Task<TimezoneApiResponseDto> GetTimezoneAsync(string city);
    Task<AstronomyApiResponseDto> GetAstronomyAsync(string city);
}