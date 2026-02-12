using WeatherBot.Api.Models;

namespace WeatherBot.Api.Services;

public interface IWeatherClient
{
    Task<WeatherSnapshot> GetCurrentAsync(string location, CancellationToken cancellationToken);
}
