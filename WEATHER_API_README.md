# Weather API Feature Documentation

## Overview
This feature adds weather information retrieval capability to the Skillsheet API. It integrates with the OpenWeatherMap API to provide real-time weather data.

## Components Added

### 1. **IWeatherService Interface**
Located in: `SkillSheetAPI.Services/Interfaces/IWeatherService.cs`

**Methods:**
- `GetWeatherByCityAsync(string city)` - Retrieve weather for a city by name
- `GetWeatherByCoordinatesAsync(double latitude, double longitude)` - Retrieve weather by geographic coordinates

**WeatherData Model:**
- City
- Country
- Temperature (°C)
- Feels Like Temperature (°C)
- Weather Description
- Humidity (%)
- Wind Speed (m/s)
- Pressure (hPa)
- Cloudiness (%)
- Timestamp

### 2. **WeatherService Implementation**
Located in: `SkillSheetAPI.Services/Services/WeatherService.cs`

- Implements IWeatherService interface
- Makes HTTP requests to OpenWeatherMap API
- Parses JSON responses
- Handles exceptions with meaningful error messages

### 3. **WeatherController**
Located in: `SkillSheetAPI/Controllers/WeatherController.cs`

**Endpoints:**

#### GET `/api/weather/city`
Query Parameters:
- `city` (required) - City name

Example: `GET /api/weather/city?city=London`

Response:
```json
{
  "message": "Weather data retrieved successfully.",
  "data": {
    "city": "London",
    "country": "GB",
    "temperature": 15.5,
    "feelsLike": 14.8,
    "description": "partly cloudy",
    "humidity": 65,
    "windSpeed": 3.5,
    "pressure": 1013,
    "clouds": 40,
    "timestamp": "2026-02-26T05:45:00Z"
  }
}
```

#### GET `/api/weather/coordinates`
Query Parameters:
- `latitude` (required) - Latitude coordinate (-90 to 90)
- `longitude` (required) - Longitude coordinate (-180 to 180)

Example: `GET /api/weather/coordinates?latitude=51.5074&longitude=-0.1278`

## Setup Instructions

1. **Obtain API Key:**
   - Register at https://openweathermap.org/api
   - Get your free API key

2. **Configuration:**
   - Add the API key to your appsettings.json or environment variables
   - Update Startup.cs/Program.cs to register the service:
   
```csharp
services.AddHttpClient<IWeatherService, WeatherService>()
    .ConfigureHttpClient(client => 
    {
        client.BaseAddress = new Uri("https://api.openweathermap.org");
    });

// Or inject directly with API key
services.AddScoped<IWeatherService>(sp => 
    new WeatherService(
        sp.GetRequiredService<HttpClient>(), 
        configuration["OpenWeatherMap:ApiKey"]
    )
);
```

## Error Handling
- Invalid city names return 400 Bad Request
- Invalid coordinates return 400 Bad Request
- API failures return 400 Bad Request with descriptive messages

## Future Enhancements
- Add forecast data (5-day, 16-day)
- Implement caching to reduce API calls
- Add support for multiple units (Fahrenheit, Kelvin)
- Add weather alerts
- Implement rate limiting

## Testing
To test the endpoints:

```
GET http://localhost:5000/api/weather/city?city=Paris
GET http://localhost:5000/api/weather/coordinates?latitude=48.8566&longitude=2.3522
```
