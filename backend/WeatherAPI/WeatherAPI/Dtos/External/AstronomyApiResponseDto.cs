using System.Text.Json.Serialization;

namespace WeatherAPI.Dtos.External;

public class AstronomyApiResponseDto
{
    [JsonPropertyName("astronomy")]
    public AstronomyWrapperDto Astronomy { get; set; } = new();
}

public class AstronomyWrapperDto
{
    [JsonPropertyName("astro")]
    public AstronomyDetailsApiDto Astro { get; set; } = new();
}

public class AstronomyDetailsApiDto
{
    [JsonPropertyName("sunrise")]
    public string Sunrise { get; set; } = string.Empty;

    [JsonPropertyName("sunset")]
    public string Sunset { get; set; } = string.Empty;

    [JsonPropertyName("moonrise")]
    public string Moonrise { get; set; } = string.Empty;

    [JsonPropertyName("moonset")]
    public string Moonset { get; set; } = string.Empty;

    [JsonPropertyName("moon_phase")]
    public string MoonPhase { get; set; } = string.Empty;
}
