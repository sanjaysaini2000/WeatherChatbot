using WeatherBot.Api.Models;
using WeatherBot.Api.Services;
using DotNetEnv;


// Load environment variables from .env
Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Bind WeatherApiOptions from configuration and environment
builder.Services.Configure<WeatherApiOptions>(options =>
{
    builder.Configuration.GetSection("WeatherApi").Bind(options);
    var envApiKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY");
    if (!string.IsNullOrWhiteSpace(envApiKey))
    {
        options.ApiKey = envApiKey;
    }
});
builder.Services.AddHttpClient<IWeatherClient, WeatherApiClient>();
builder.Services.AddSingleton<LocationParser>();
builder.Services.AddScoped<WeatherChatService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("frontend");

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/api/chat", async (ChatRequest request, WeatherChatService chatService, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(request.Message))
    {
        return Results.BadRequest(new { error = "Message is required." });
    }

    try
    {
        var response = await chatService.HandleAsync(request, ct);
        return Results.Ok(response);
    }
    catch (WeatherApiException ex)
    {
        return Results.Problem(ex.Message, statusCode: ex.StatusCode);
    }
}).WithName("Chat");

app.Run();
