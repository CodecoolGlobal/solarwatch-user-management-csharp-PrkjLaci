namespace SolarWatch.Service.Authentication;

public interface IAuthenticationSeeder
{
    void AddRoles();
    void AddAdmin();
}