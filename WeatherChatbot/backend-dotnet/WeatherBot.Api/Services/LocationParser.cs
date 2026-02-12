using System.Text.RegularExpressions;

namespace WeatherBot.Api.Services;

public sealed class LocationParser
{
    private static readonly Regex WeatherInRegex = new(@"\b(?:weather|forecast)\s+(?:in|for)\s+(.+)$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex SimpleInRegex = new(@"\b(?:in|at|for)\s+(.+)$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public string ExtractLocation(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return string.Empty;
        }

        var trimmed = message.Trim();
        var match = WeatherInRegex.Match(trimmed);
        if (!match.Success)
        {
            match = SimpleInRegex.Match(trimmed);
        }

        var candidate = match.Success ? match.Groups[1].Value : trimmed;
        return TrimTrailingPunctuation(candidate);
    }

    private static string TrimTrailingPunctuation(string value)
    {
        return value.Trim().TrimEnd('.', '?', '!', ',', ';', ':');
    }
}
