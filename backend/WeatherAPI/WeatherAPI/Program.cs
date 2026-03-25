using WeatherAPI.Services;
using WeatherAPI.Services.Interfaces;
using WeatherAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WeatherApiOptions>(
    builder.Configuration.GetSection("WeatherApi"));

builder.Services.AddHttpClient<IWeatherService, WeatherService>();

//builder.Services.AddSingleton<IWeatherService, WeatherService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();