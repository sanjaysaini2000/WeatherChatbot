namespace WeatherBot.Api.Services;

public sealed class WeatherApiException : Exception
{
    public int StatusCode { get; }

    public WeatherApiException(string message, int statusCode = 502) : base(message)
    {
        StatusCode = statusCode;
    }
}
