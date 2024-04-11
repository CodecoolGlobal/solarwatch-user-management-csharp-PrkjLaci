using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Service.Authentication;

public class AuthenticationSeeder : IAuthenticationSeeder
{
    private RoleManager<IdentityRole> roleManager;
    private UserManager<IdentityUser> userManager;
    private IConfiguration configuration;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
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
    
    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }
    
    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
        if (adminInDb == null)
        {
            var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
            var adminCreated = await userManager.CreateAsync(admin, "admin123");

            if (adminCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}