using SolarWatch.Service.Geocoding;
using SolarWatch.Service.SunsetSunRise;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Data;
using SolarWatch.Repository.CityRepository;
using SolarWatch.Repository.SunsetSunriseRepository;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DotEnv.Load();

AddServices();
ConfigureSwagger();
AddAuthentication();
AddIdentity();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void AddServices()
{
    builder.Services.AddHttpClient();
    builder.Services.AddControllers(
        options =>
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        });
    builder.Services.AddSingleton<IGeocodingApiProvider, GeocodingApiProvider>();
    builder.Services.AddSingleton<ICityCoordinatesJsonProcessor, CityCoordinatesJsonProcessor>();
    builder.Services.AddSingleton<ICityDataRepository, CityDataRepository>();
    builder.Services.AddSingleton<ISunsetSunriseApiProvider, SunsetSunriseApiProvider>();
    builder.Services.AddSingleton<ISunsetSunriseJsonProcessor, SunsetSunriseJsonProcessor>();
    builder.Services.AddSingleton<ISunsetSunriseRepository, SunsetSunriseRepository>();
    builder.Services.AddEndpointsApiExplorer();
}

void ConfigureSwagger()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

void AddDbContext()
{
    builder.Services.AddDbContext<SolarWatchContext>();
}

void AddAuthentication()
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
}

void AddIdentity()
{
    
}