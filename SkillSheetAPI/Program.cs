using DataAccess.Entities.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Repositories;
using SkillSheetAPI.Services.Services;
using SkillSheetAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SkillSheetAPI.MapperProfiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("dbms")));

//Register repo and service
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISkillDataService, SkillDataService>();
builder.Services.AddScoped<IUserDetailRepo, UserDetailRepo>();
builder.Services.AddScoped<ISkillDataRepo, SkillDataRepo>();
builder.Services.AddScoped<IUserDetailService, UserDetailService>();
builder.Services.AddScoped<ISkillDataRepo, SkillDataRepo>();
builder.Services.AddScoped<IUserDetailService, UserDetailService>();
builder.Services.AddScoped<IUserSkillRepo, UserSkillRepo>();
builder.Services.AddScoped<IUserSkillService, UserSkillService>();

// Register AutoMapper profiles
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));
builder.Services.AddAutoMapper(typeof(UserDetailMappingProfile));
builder.Services.AddAutoMapper(typeof(SkillDataMappingProfile));
builder.Services.AddAutoMapper(typeof(UserSkillMappingProfile));

var jwtSecret = builder.Configuration["Jwt:SecretKey"];

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
           };
       });


// Configure JWT authentication
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer <your_token>' to authenticate"
    });

  

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {} // No specific scopes required
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin")); // Ensure this matches the role in your token
});

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
app.UseCors("AllowAllOrigins");
app.UseCors("AllowSpecificOrigin");
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/wwwroot"
});

app.MapControllers();
app.Run();
