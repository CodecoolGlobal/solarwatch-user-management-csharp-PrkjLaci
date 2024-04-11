using System.Data.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SolarWatch.Data;
using SolarWatch.IntegrationTests.Authentication;
using SolarWatch.Models;
using SolarWatch.Service.Authentication;

namespace SolarWatch.IntegrationTests;

public class SolarWatchWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly string _dbName = Guid.NewGuid().ToString();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarWatchContext>));
            var usersDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UsersContext>));
            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            var authenticationSeederDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthenticationSeeder));
            
            services.Remove(solarWatchDbContextDescriptor);
            services.Remove(usersDbContextDescriptor);
            services.Remove(dbConnectionDescriptor);
            services.Remove(authenticationSeederDescriptor);
            
            services.AddDbContext<SolarWatchContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            services.AddDbContext<UsersContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            services.AddScoped<IAuthenticationSeeder, FakeAuthenticationSeeder>();
            
            
            using var scope = services.BuildServiceProvider().CreateScope();
            //SeedUsersDatabase(services);
            
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.Configuration = new OpenIdConnectConfiguration
                    {
                        Issuer = JwtTokenHelper.Issuer,
                    };
                    options.TokenValidationParameters.ValidIssuer = JwtTokenHelper.Issuer;
                    options.TokenValidationParameters.ValidAudience = JwtTokenHelper.Issuer;
                    options.TokenValidationParameters.IssuerSigningKey = JwtTokenHelper.SecurityKey;
                }
            );
            SeedSolarWatchDatabase(scope);
        });
    }


    private static async Task SeedSolarWatchDatabase(IServiceScope serviceScope)
    {
        var solarWatchContext = serviceScope.ServiceProvider.GetRequiredService<SolarWatchContext>();
        var scopedService = serviceScope.ServiceProvider;
        var db = scopedService.GetRequiredService<SolarWatchContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();

        solarWatchContext.CityData.Add(
            new City
            {
                Id = 1,
                CityName = "Budapest",
                Latitude = 47.4979,
                Longitude = 19.0402,
                State = null,
                Country = "Hungary",
                SunsetSunriseTime = new List<SunsetSunriseTime>()
            }
        );
        
        solarWatchContext.AddRange(new List<SunsetSunriseTime>
        {
            new()
            {
                Id = 1,
                Date = "2024-04-10",
                Sunset = "18:00",
                Sunrise = "06:00",
                CityId = 1
            },
            new()
            {
                Id = 2,
                Date = "2024-04-11",
                Sunset = "18:01",
                Sunrise = "05:59",
                CityId = 1
            }
        });
        await solarWatchContext.SaveChangesAsync();
    }
    
    private static async Task SeedUsersDatabase(IServiceScope serviceScope)
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<UsersContext>();
        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        var authenticationSeeder = serviceScope.ServiceProvider.GetRequiredService<FakeAuthenticationSeeder>();
        authenticationSeeder.AddRoles();
        authenticationSeeder.AddAdmin();
        
        context.Users.Add(
            new()
            {
                Id = "1",
                UserName = "testUser",
                PasswordHash = "testPassword",
                Email = "test@test.com"
            }
        ); 
        await context.SaveChangesAsync();
    }
}