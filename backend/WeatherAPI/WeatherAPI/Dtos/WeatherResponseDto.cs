namespace WeatherAPI.Dtos;

// Response DTO returned by the API.
// Aggregates weather, timezone, and astronomy data from the RapidAPI endpoints
// into a single response to the client
public class WeatherResponseDto
{
    public string City { get; set; } = string.Empty;
    public WeatherDetailsDto Weather { get; set; } = new();
    public TimezoneDetailsDto Timezone { get; set; } = new();
    public AstronomyDetailsDto Astronomy { get; set; } = new();
}

public class WeatherDetailsDto
{
    public double TemperatureC { get; set; }
    public string Condition { get; set; } = string.Empty;
    public double WindKph { get; set; }
    public int Humidity { get; set; }
    public int Cloud { get; set; }
    public bool IsDay { get; set; }
    public string Icon { get; set; } = string.Empty;
}

public class TimezoneDetailsDto
{
    public string TimezoneId { get; set; } = string.Empty;
    public string LocalTime { get; set; } = string.Empty;
    public long LocaltimeEpoch { get; set; }
}

public class AstronomyDetailsDto
{
    public string Sunrise { get; set; } = string.Empty;
    public string Sunset { get; set; } = string.Empty;
    public string Moonrise { get; set; } = string.Empty;
    public string Moonset { get; set; } = string.Empty;
    public string MoonPhase { get; set; } = string.Empty;
}