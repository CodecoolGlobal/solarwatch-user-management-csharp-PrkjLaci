using SolarWatch.Service.Authentication;

namespace SolarWatch.IntegrationTests.Authentication;

public class FakeAuthenticationSeeder : IAuthenticationSeeder
{
    public void AddRoles()
    {
        Console.WriteLine("Adding roles");
        return;
    }

    public void AddAdmin()
    {
        return;
    }
}