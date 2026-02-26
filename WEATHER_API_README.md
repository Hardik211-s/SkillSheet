# Weather API - Setup Instructions

## Prerequisites
- .NET 6.0 or higher
- Visual Studio 2022 or VS Code
- OpenWeatherMap API key (free)

---

## Quick Setup

### 1. Get API Key
Visit: https://openweathermap.org/api
- Sign up for free account
- Get your API key

### 2. Set API Key
```bash
dotnet user-secrets set "OpenWeatherMap:ApiKey" "your_api_key_here"
```

### 3. Update Program.cs
Add this to your `Program.cs` (before `app.Run()`):

```csharp
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Add Weather Service
var apiKey = builder.Configuration["OpenWeatherMap:ApiKey"] 
    ?? Environment.GetEnvironmentVariable("OpenWeatherMap__ApiKey")
    ?? throw new InvalidOperationException("API key not configured");

builder.Services.AddScoped<IWeatherService>(sp =>
    new WeatherService(new HttpClient(), apiKey)
);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### 4. Update appsettings.json
Add this section:
```json
{
  "OpenWeatherMap": {
    "ApiKey": ""
  }
}
```

### 5. Build & Run
```bash
dotnet clean
dotnet restore
dotnet build
dotnet run
```

---

## API Endpoints

### Get Weather by City
```
GET http://localhost:5000/api/weather/city?city=London
```

### Get Weather by Coordinates
```
GET http://localhost:5000/api/weather/coordinates?latitude=51.5074&longitude=-0.1278
```

---

## Testing with cURL

```bash
# City
curl "http://localhost:5000/api/weather/city?city=Paris"

# Coordinates
curl "http://localhost:5000/api/weather/coordinates?latitude=48.8566&longitude=2.3522"
```

---

## Response Format
```json
{
  "message": "Weather data retrieved successfully.",
  "data": {
    "city": "London",
    "country": "GB",
    "temperature": 15.5,
    "feelsLike": 14.8,
    "description": "cloudy",
    "humidity": 65,
    "windSpeed": 3.5,
    "pressure": 1013,
    "clouds": 40,
    "timestamp": "2026-02-26T10:00:00Z"
  }
}
```

---

## Troubleshooting

**Error: "API key not configured"**
- Run: `dotnet user-secrets set "OpenWeatherMap:ApiKey" "your_key"`

**Error: "Port 5000 in use"**
- Edit Program.cs and change port

**Error: "Service not registered"**
- Add service registration to Program.cs

**Error: Build fails**
- Delete bin and obj folders
- Run: `dotnet restore`

---

## What Changed in This Fix

✅ Removed ILogger dependency  
✅ Simplified WeatherService  
✅ Removed response DTOs  
✅ No external dependencies needed  
✅ Works out of the box  

---

**Ready to use! Just follow the steps above.** 🚀
