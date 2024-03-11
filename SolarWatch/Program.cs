using SolarWatch.Service.Geocoding;
using SolarWatch.Service.SunsetSunRise;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IGeocodingApiProvider, GeocodingApiProvider>();
builder.Services.AddTransient<ICityCoordinatesJsonProcessor, CityCoordinatesJsonProcessor>();
builder.Services.AddTransient<ISunsetSunriseApiProvider, SunsetSunriseApiProvider>();
builder.Services.AddTransient<ISunsetSunriseJsonProcessor, SunsetSunriseJsonProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();