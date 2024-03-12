using SolarWatch.Service.Geocoding;
using SolarWatch.Service.SunsetSunRise;
using dotenv.net;
using SolarWatch.Repository.City;

var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IGeocodingApiProvider, GeocodingApiProvider>();
builder.Services.AddTransient<ICityCoordinatesJsonProcessor, CityCoordinatesJsonProcessor>();
builder.Services.AddTransient<ICityDataRepository, CityDataRepository>();
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