namespace WeatherBot.Api.Services;

public sealed class WeatherApiOptions
{
    public string BaseUrl { get; init; } = "https://api.weatherapi.com/v1";
    public string ApiKey { get; init; } = string.Empty;
}
