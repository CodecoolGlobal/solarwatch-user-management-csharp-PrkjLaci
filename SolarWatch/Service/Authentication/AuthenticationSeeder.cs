using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Service.Authentication;

public class AuthenticationSeeder
{
    private RoleManager<IdentityRole> roleManager;
    private IConfiguration configuration;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        this.roleManager = roleManager;
        this.configuration = configuration;
    }

    public void AddRoles()
    {
        var tAdmin = CreateAdminRole(roleManager);
        tAdmin.Wait();
        
        var tUser = CreateUserRole(roleManager);
        tUser.Wait();
    }

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(configuration["Roles:AdminRoleName"]));
    }

    private async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(configuration["Roles:UserRoleName"]));
    }
}