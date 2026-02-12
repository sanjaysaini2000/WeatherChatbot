namespace WeatherBot.Api.Models;

public record ChatResponse(
    string Reply,
    string Location,
    string Condition,
    string IconUrl,
    double TemperatureC,
    double TemperatureF,
    double FeelsLikeC,
    double FeelsLikeF,
    int Humidity,
    double WindKph,
    string LocalTime,
    string ObservedAt
);
