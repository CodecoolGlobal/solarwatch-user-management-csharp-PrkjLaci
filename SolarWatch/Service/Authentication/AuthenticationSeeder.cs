using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Service.Authentication;

public class AuthenticationSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public void AddRoles()
    {
        var tAdmin = CreateAdminRole(_roleManager);
        tAdmin.Wait();
        
        var tUser = CreateUserRole(_roleManager);
        tUser.Wait();
    }

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(_configuration["Roles:AdminRoleName"]));
    }

    private async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(_configuration["Roles:UserRoleName"]));
    }
}