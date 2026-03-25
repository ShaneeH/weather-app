namespace WeatherAPI.Configuration;

public class WeatherApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
}