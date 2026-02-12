# Weather Chatbot

A full-stack weather assistant with a dedicated chat UI in React and a .NET (ASP.NET Core) backend. The backend calls the WeatherAPI current weather endpoint and returns live conditions, temperature, humidity, and wind details.

## Features

- Chat-style UI that accepts natural prompts like "What is the weather in Paris?"
- Real-time weather lookups via the WeatherAPI current conditions endpoint
- Clean service-based backend design (API client, parser, chat service)
- Easy to extend with forecasts or additional data sources

## Setup

1. Backend (ASP.NET Core)

```
cd backend-dotnet/WeatherBot.Api
setx WeatherApi__ApiKey "YOUR_WEATHERAPI_KEY"
dotnet run
```

The API will be available at `http://localhost:5000` by default.

2. Frontend (React)

```
cd frontend
npm install
npm start
```

The UI will run at `http://localhost:3000` and proxy API requests to the backend.

## Configuration

- `WeatherApi__ApiKey` (required): Your WeatherAPI key.
- `WeatherApi__BaseUrl` (optional): Override the default WeatherAPI base URL.
- `REACT_APP_API_BASE` (optional): Override the API base URL for the frontend.

## API

- `POST /api/chat`

Request body:
```
{
  "message": "What is the weather in Seattle?"
}
```

Response body:
```
{
  "reply": "Right now in Seattle, WA it's 8.1°C (46.6°F) with Partly cloudy...",
  "location": "Seattle, WA",
  "condition": "Partly cloudy",
  "iconUrl": "https://...",
  "temperatureC": 8.1,
  "temperatureF": 46.6,
  "feelsLikeC": 6.2,
  "feelsLikeF": 43.2,
  "humidity": 71,
  "windKph": 12.4,
  "localTime": "2026-02-08 17:42",
  "observedAt": "2026-02-08 17:30"
}
```

## Notes

- The backend reads its configuration from `backend-dotnet/WeatherBot.Api/appsettings.json` and environment variables.
- The WeatherAPI documentation specifies the current weather endpoint and required query parameters.
