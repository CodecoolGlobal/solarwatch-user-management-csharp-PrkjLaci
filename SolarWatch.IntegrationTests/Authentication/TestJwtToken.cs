using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SolarWatch.IntegrationTests.Authentication;

public class TestJwtToken
{
    private List<Claim> Claims { get; } = new();
    private int ExpiresInMinutes { get; set; } = 30;

    public TestJwtToken WithRole(string roleName)
    {
        Claims.Add(new Claim(ClaimTypes.Role, roleName));
        return this;
    }
    
    public TestJwtToken WithName(string username)
    {
        Claims.Add(new Claim(ClaimTypes.Name, username));
        return this;
    }

    public string Build()
    {
        var token = new JwtSecurityToken(
            JwtTokenHelper.Issuer,
            JwtTokenHelper.Issuer,
            Claims,
            expires: DateTime.Now.AddMinutes(ExpiresInMinutes),
            signingCredentials: JwtTokenHelper.SigningCredentials
        );
        return JwtTokenHelper.JwtSecurityTokenHandler.WriteToken(token);
    }
}