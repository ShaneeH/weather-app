using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Services;

public class WeatherService : IWeatherService
{
    private static readonly string[] AvailableCities =
    [
        "Dublin",
        "Tokyo",
        "Chicago"
    ];

    public IReadOnlyCollection<string> GetAvailableCities()
    {
        return AvailableCities;
    }
}