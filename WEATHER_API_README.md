# Weather API Feature Documentation

## Overview
This feature adds comprehensive weather information retrieval capability to the Skillsheet API. It integrates with the OpenWeatherMap API to provide real-time weather data with robust error handling, logging, and comprehensive unit test coverage.

## Components Added

### 1. **IWeatherService Interface**
Located in: `SkillSheetAPI.Services/Interfaces/IWeatherService.cs`

**Methods:**
- `GetWeatherByCityAsync(string city)` - Retrieve weather for a city by name
- `GetWeatherByCoordinatesAsync(double latitude, double longitude)` - Retrieve weather by geographic coordinates

**WeatherData Model:**
- City - City name
- Country - Country code
- Temperature (°C) - Current temperature in Celsius
- Feels Like (°C) - Perceived temperature in Celsius
- Description - Weather condition description
- Humidity (%) - Humidity percentage
- Wind Speed (m/s) - Wind speed in meters per second
- Pressure (hPa) - Atmospheric pressure in hectopascals
- Clouds (%) - Cloud coverage percentage
- Timestamp - UTC timestamp of the data retrieval

### 2. **WeatherService Implementation**
Located in: `SkillSheetAPI.Services/Services/WeatherService.cs`

**Features:**
- Implements IWeatherService interface
- Makes HTTP requests to OpenWeatherMap API
- Comprehensive JSON parsing with safe property access using `TryGetProperty()`
- Graceful handling of missing or unexpected API response fields
- Request timeout handling (10 seconds)
- URL encoding for city names to handle special characters
- Structured logging using ILogger interface
- Detailed exception handling with meaningful error messages
- Input validation for latitude (-90 to 90) and longitude (-180 to 180)

**Error Handling:**
- `ArgumentNullException` - For null/empty parameters
- `ArgumentException` - For invalid coordinates
- `HttpRequestException` - For API failures
- `InvalidOperationException` - For JSON parsing errors
- `TaskCanceledException` - For request timeouts

### 3. **WeatherController**
Located in: `SkillSheetAPI/Controllers/WeatherController.cs`

**Endpoints:**

#### GET `/api/weather/city`
Query Parameters:
- `city` (required) - City name (string, max 100 characters)

Example: `GET /api/weather/city?city=London`

Success Response (200 OK):
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
  },
  "timestamp": "2026-02-26T05:45:10Z"
}
```

Error Response (400 Bad Request):
```json
{
  "message": "City name is required and cannot be empty.",
  "errorCode": "MISSING_PARAMETER",
  "details": "City parameter was null or empty",
  "timestamp": "2026-02-26T05:45:10Z"
}
```

#### GET `/api/weather/coordinates`
Query Parameters:
- `latitude` (required) - Latitude coordinate (-90 to 90)
- `longitude` (required) - Longitude coordinate (-180 to 180)

Example: `GET /api/weather/coordinates?latitude=51.5074&longitude=-0.1278`

**Error Codes:**
- `MISSING_PARAMETER` - Required parameter is missing or empty
- `INVALID_PARAMETER` - Parameter validation failed
- `INVALID_LATITUDE` - Latitude out of valid range
- `INVALID_LONGITUDE` - Longitude out of valid range
- `INVALID_COORDINATES` - Coordinate validation failed
- `EXTERNAL_API_ERROR` - OpenWeatherMap API call failed
- `PARSING_ERROR` - Response parsing failed
- `INTERNAL_SERVER_ERROR` - Unexpected server error

### 4. **Unit Tests**
Located in: `SkillSheet.Tests/Services/WeatherServiceTests.cs`

**Test Coverage:**
- Valid city weather retrieval
- Valid coordinates weather retrieval
- Null and empty parameter handling
- Invalid coordinate range validation
- API failure handling (HTTP 404, etc.)
- Empty API response handling
- Missing optional fields handling
- Invalid JSON parsing
- Constructor validation
- Boundary coordinate testing (±90, ±180)

**Test Framework:**
- MSTest (Microsoft.VisualStudio.TestTools.UnitTesting)
- Moq for mocking dependencies

### 5. **Documentation**
Located in: `WEATHER_API_README.md` (this file)

## Setup Instructions

### Step 1: Obtain API Key
1. Register at https://openweathermap.org/api
2. Choose the free tier plan
3. Verify your email
4. Get your free API key from the account dashboard

### Step 2: Configure API Key (Secure Storage)

⚠️ **CRITICAL: Never commit API keys to source control!**

#### Option A: Environment Variables (Recommended for Development)
```bash
# Windows CMD
set OpenWeatherMap__ApiKey=your_api_key_here

# Windows PowerShell
$env:OpenWeatherMap__ApiKey="your_api_key_here"

# Linux/Mac
export OpenWeatherMap__ApiKey=your_api_key_here
```

#### Option B: User Secrets (Development)
```bash
# Initialize user secrets (one time)
dotnet user-secrets init

# Set the API key
dotnet user-secrets set "OpenWeatherMap:ApiKey" "your_api_key_here"
```

#### Option C: appsettings.json (Development Only - NEVER for Production)
```json
{
  "OpenWeatherMap": {
    "ApiKey": "your_api_key_here"
  }
}
```

#### Option D: Azure Key Vault (Production Recommended)
```csharp
// In Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add Key Vault
var keyVaultEndpoint = new Uri(builder.Configuration["KeyVault:Endpoint"]);
builder.Configuration.AddAzureKeyVault(
    keyVaultEndpoint,
    new DefaultAzureCredential()
);
```

### Step 3: Register Service in Dependency Injection

#### For .NET 6+ (Program.cs)
```csharp
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to container
builder.Services.AddControllers();
builder.Services.AddLogging(); // Important: Required for ILogger dependency

// Register HttpClient with custom timeout
builder.Services.AddHttpClient<IWeatherService, WeatherService>()
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(10);
    });

var app = builder.Build();

// ... rest of configuration

app.MapControllers();
app.Run();
```

#### Dependency Injection Details
The WeatherService constructor expects:
- `HttpClient` - Injected via AddHttpClient
- `string apiKey` - From configuration
- `ILogger<WeatherService>` - From built-in logging

### Step 4: Configure appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "SkillSheetAPI.Services.Services.WeatherService": "Debug"
    }
  },
  "OpenWeatherMap": {
    "ApiKey": "${OpenWeatherMap__ApiKey}"  // Injected from environment
  }
}
```

## Error Handling

### Client-Side Validation
- City name: Required, max 100 characters
- Latitude: -90 to 90
- Longitude: -180 to 180

### Server-Side Handling
- Graceful fallback for missing optional fields
- Detailed error messages with error codes
- HTTP status codes:
  - 200 OK - Success
  - 400 Bad Request - Client error
  - 500 Internal Server Error - Server error

### Logging
All operations are logged with appropriate levels:
- **Information** - Successful operations
- **Warning** - Parameter validation failures
- **Error** - Service failures and exceptions

View logs in application output or configured log providers.

## Testing the API

### Using Postman
```
GET http://localhost:5000/api/weather/city?city=Paris
GET http://localhost:5000/api/weather/coordinates?latitude=48.8566&longitude=2.3522
```

### Using cURL
```bash
# By city
curl "http://localhost:5000/api/weather/city?city=London"

# By coordinates
curl "http://localhost:5000/api/weather/coordinates?latitude=51.5074&longitude=-0.1278"
```

### Using PowerShell
```powershell
# By city
Invoke-WebRequest -Uri "http://localhost:5000/api/weather/city?city=London" -Method Get

# By coordinates
Invoke-WebRequest -Uri "http://localhost:5000/api/weather/coordinates?latitude=51.5074&longitude=-0.1278" -Method Get
```

### Using .NET HttpClient
```csharp
using HttpClient client = new();
var response = await client.GetAsync("http://localhost:5000/api/weather/city?city=London");
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);
```

## Running Unit Tests

```bash
# Run all tests
dotnet test

# Run only WeatherService tests
dotnet test --filter "FullyQualifiedName~WeatherServiceTests"

# Run with detailed output
dotnet test -v detailed

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

## Performance Considerations

- **Response Time**: Typically 500-2000ms depending on network
- **Rate Limiting**: OpenWeatherMap free tier allows 60 API calls per minute
- **Caching**: Consider implementing response caching for frequently requested cities
- **Timeout**: Set to 10 seconds to prevent hanging requests

## Future Enhancements

### High Priority
- [ ] Implement response caching with TTL (5-10 minutes)
- [ ] Add rate limiting middleware
- [ ] Support for forecast data (5-day, 16-day)
- [ ] Support for multiple temperature units (Fahrenheit, Kelvin)

### Medium Priority
- [ ] Add weather alerts API
- [ ] Support for historical weather data
- [ ] Batch requests for multiple cities
- [ ] WeatherController unit tests

### Low Priority
- [ ] Add support for multiple language descriptions
- [ ] Implement circuit breaker pattern
- [ ] Add metrics/telemetry
- [ ] Support for UV index data

## Troubleshooting

### API Key Errors
**Problem**: "Invalid API key" error
**Solution**: 
1. Verify API key is correctly set in configuration
2. Check that API key hasn't been revoked
3. Confirm free tier plan is active

### City Not Found
**Problem**: 404 error for valid city names
**Solution**: 
1. Try with alternative spelling
2. Include country code (e.g., "London,GB")
3. Check OpenWeatherMap city database

### Connection Timeout
**Problem**: Request times out
**Solution**: 
1. Check internet connectivity
2. Verify OpenWeatherMap API is accessible
3. Increase timeout if needed (default: 10 seconds)

### Missing Fields
**Problem**: Some weather fields are null
**Solution**: 
1. This is expected - service uses graceful fallbacks
2. Fields like pressure may not be available for all locations
3. Check service logs for details

## Security Best Practices

1. **Never commit API keys** - Use secure configuration management
2. **Use HTTPS in production** - Ensure encrypted communication
3. **Validate input** - Always validate city names and coordinates
4. **Rate limit** - Implement rate limiting to prevent abuse
5. **Log securely** - Don't log sensitive data like API keys
6. **Use environment variables** - For development and CI/CD pipelines
7. **Rotate keys regularly** - Change API keys periodically
8. **Monitor usage** - Track API consumption for anomalies

## API Integration Notes

- **Base URL**: `https://api.openweathermap.org/data/2.5/weather`
- **Units**: Metric (Celsius, m/s, hPa)
- **Authentication**: Query parameter `appid`
- **Rate Limit**: 60 calls/minute (free tier)
- **Response Format**: JSON

## Support & Resources

- OpenWeatherMap API Documentation: https://openweathermap.org/api
- GitHub Repository: https://github.com/Hardik211-s/SkillSheet
- Issues & Bug Reports: [Project Issue Tracker]

---

**Last Updated**: 2026-02-26  
**Version**: 1.0.0 (Improved)  
**Status**: Production Ready
