namespace WeatherAPI.Services.Interfaces;

public interface IWeatherService
{
    IReadOnlyCollection<string> GetAvailableCities();
}