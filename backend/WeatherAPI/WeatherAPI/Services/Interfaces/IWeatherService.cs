using WeatherAPI.Dtos;

namespace WeatherAPI.Services.Interfaces;

public interface IWeatherService
{
    IReadOnlyCollection<string> GetAvailableCities();
    bool IsSupportedCity(string city);
    Task<WeatherResponseDto> GetWeatherByCityAsync(string city);
}