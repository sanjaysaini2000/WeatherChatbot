using System.Text.Json.Serialization;

namespace WeatherBot.Api.Models;

public sealed record WeatherApiResponse(
    [property: JsonPropertyName("location")] WeatherApiLocation Location,
    [property: JsonPropertyName("current")] WeatherApiCurrent Current
);

public sealed record WeatherApiLocation(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("region")] string Region,
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("localtime")] string LocalTime
);

public sealed record WeatherApiCurrent(
    [property: JsonPropertyName("temp_c")] double TempC,
    [property: JsonPropertyName("temp_f")] double TempF,
    [property: JsonPropertyName("feelslike_c")] double FeelsLikeC,
    [property: JsonPropertyName("feelslike_f")] double FeelsLikeF,
    [property: JsonPropertyName("humidity")] int Humidity,
    [property: JsonPropertyName("wind_kph")] double WindKph,
    [property: JsonPropertyName("last_updated")] string LastUpdated,
    [property: JsonPropertyName("condition")] WeatherApiCondition Condition
);

public sealed record WeatherApiCondition(
    [property: JsonPropertyName("text")] string Text,
    [property: JsonPropertyName("icon")] string Icon
);

public sealed record WeatherApiErrorResponse(
    [property: JsonPropertyName("error")] WeatherApiError Error
);

public sealed record WeatherApiError(
    [property: JsonPropertyName("code")] int Code,
    [property: JsonPropertyName("message")] string Message
);
