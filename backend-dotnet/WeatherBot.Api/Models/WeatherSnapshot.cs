namespace WeatherBot.Api.Models;

public record WeatherSnapshot(
    string LocationName,
    string Region,
    string Country,
    string LocalTime,
    string Condition,
    string IconUrl,
    double TemperatureC,
    double TemperatureF,
    double FeelsLikeC,
    double FeelsLikeF,
    int Humidity,
    double WindKph,
    string LastUpdated
);
