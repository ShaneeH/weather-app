using System.Text.Json.Serialization;

namespace WeatherAPI.Dtos.External;

public class TimezoneApiResponseDto
{
    [JsonPropertyName("location")]
    public TimezoneLocationDto Location { get; set; } = new();
}

public class TimezoneLocationDto
{
    [JsonPropertyName("tz_id")]
    public string TimezoneId { get; set; } = string.Empty;

    [JsonPropertyName("localtime")]
    public string LocalTime { get; set; } = string.Empty;

    [JsonPropertyName("localtime_epoch")]
    public long LocaltimeEpoch { get; set; }
}