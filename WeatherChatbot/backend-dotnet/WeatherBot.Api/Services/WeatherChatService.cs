using WeatherBot.Api.Models;

namespace WeatherBot.Api.Services;

public sealed class WeatherChatService
{
    private readonly IWeatherClient _weatherClient;
    private readonly LocationParser _parser;

    public WeatherChatService(IWeatherClient weatherClient, LocationParser parser)
    {
        _weatherClient = weatherClient;
        _parser = parser;
    }

    public async Task<ChatResponse> HandleAsync(ChatRequest request, CancellationToken cancellationToken)
    {
        var location = _parser.ExtractLocation(request.Message);
        if (string.IsNullOrWhiteSpace(location))
        {
            throw new WeatherApiException("Please include a location in your message, for example: 'Weather in Seattle'.", 400);
        }

        var snapshot = await _weatherClient.GetCurrentAsync(location, cancellationToken);
        var label = string.IsNullOrWhiteSpace(snapshot.Region)
            ? $"{snapshot.LocationName}, {snapshot.Country}".Trim().TrimEnd(',')
            : $"{snapshot.LocationName}, {snapshot.Region}".Trim().TrimEnd(',');

        var reply = $"Right now in {label} it's {snapshot.TemperatureC:0.#}°C ({snapshot.TemperatureF:0.#}°F) with {snapshot.Condition}. " +
                    $"Feels like {snapshot.FeelsLikeC:0.#}°C, humidity {snapshot.Humidity}% and wind {snapshot.WindKph:0.#} kph.";

        return new ChatResponse(
            reply,
            label,
            snapshot.Condition,
            snapshot.IconUrl,
            snapshot.TemperatureC,
            snapshot.TemperatureF,
            snapshot.FeelsLikeC,
            snapshot.FeelsLikeF,
            snapshot.Humidity,
            snapshot.WindKph,
            snapshot.LocalTime,
            snapshot.LastUpdated
        );
    }
}
