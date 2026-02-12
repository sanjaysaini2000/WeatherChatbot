namespace WeatherBot.Api.Services;

public sealed class WeatherApiOptions
{
    public string BaseUrl { get; set; } = "https://api.weatherapi.com/v1";
    public string ApiKey { get; set; } = string.Empty;
}
