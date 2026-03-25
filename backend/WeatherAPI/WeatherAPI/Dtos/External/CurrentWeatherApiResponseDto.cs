using System.Text.Json.Serialization;

namespace WeatherAPI.Dtos.External;

public class CurrentWeatherApiResponseDto
{
    [JsonPropertyName("location")]
    public CurrentWeatherLocationDto Location { get; set; } = new();

    [JsonPropertyName("current")]
    public CurrentWeatherDetailsDto Current { get; set; } = new();
}

public class CurrentWeatherLocationDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("tz_id")]
    public string TimezoneId { get; set; } = string.Empty;

    [JsonPropertyName("localtime_epoch")]
    public long LocaltimeEpoch { get; set; }

    [JsonPropertyName("localtime")]
    public string LocalTime { get; set; } = string.Empty;
}

public class CurrentWeatherDetailsDto
{
    [JsonPropertyName("temp_c")]
    public double TemperatureC { get; set; }

    [JsonPropertyName("condition")]
    public WeatherConditionDto Condition { get; set; } = new();

    [JsonPropertyName("wind_kph")]
    public double WindKph { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("cloud")]
    public int Cloud { get; set; }

    [JsonPropertyName("is_day")]
    public int IsDay { get; set; }
}

public class WeatherConditionDto
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public int Code { get; set; }
}