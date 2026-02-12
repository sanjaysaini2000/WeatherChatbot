using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WeatherBot.Api.Models;

namespace WeatherBot.Api.Services;

public sealed class WeatherApiClient : IWeatherClient
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;

    public WeatherApiClient(HttpClient httpClient, IOptions<WeatherApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<WeatherSnapshot> GetCurrentAsync(string location, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new WeatherApiException("Weather API key is missing. Set WeatherApi:ApiKey in configuration.", 500);
        }

        var baseUrl = string.IsNullOrWhiteSpace(_options.BaseUrl)
            ? "https://api.weatherapi.com/v1"
            : _options.BaseUrl.TrimEnd('/');

        var uri = $"{baseUrl}/current.json?key={Uri.EscapeDataString(_options.ApiKey)}&q={Uri.EscapeDataString(location)}&aqi=no";

        using var response = await _httpClient.GetAsync(uri, cancellationToken);
        var payload = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var message = TryGetErrorMessage(payload) ?? $"Weather API request failed ({(int)response.StatusCode}).";
            var status = response.StatusCode == HttpStatusCode.BadRequest ? 400 : 502;
            throw new WeatherApiException(message, status);
        }

        var data = JsonSerializer.Deserialize<WeatherApiResponse>(payload, JsonOptions)
            ?? throw new WeatherApiException("Weather API response could not be parsed.", 502);

        var iconUrl = data.Current.Condition.Icon ?? string.Empty;
        if (iconUrl.StartsWith("//", StringComparison.Ordinal))
        {
            iconUrl = "https:" + iconUrl;
        }

        return new WeatherSnapshot(
            data.Location.Name,
            data.Location.Region,
            data.Location.Country,
            data.Location.LocalTime,
            data.Current.Condition.Text,
            iconUrl,
            data.Current.TempC,
            data.Current.TempF,
            data.Current.FeelsLikeC,
            data.Current.FeelsLikeF,
            data.Current.Humidity,
            data.Current.WindKph,
            data.Current.LastUpdated
        );
    }

    private static string? TryGetErrorMessage(string payload)
    {
        try
        {
            var error = JsonSerializer.Deserialize<WeatherApiErrorResponse>(payload, JsonOptions);
            return error?.Error?.Message;
        }
        catch
        {
            return null;
        }
    }
}
