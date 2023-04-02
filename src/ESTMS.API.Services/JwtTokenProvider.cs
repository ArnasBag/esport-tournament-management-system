using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ESTMS.API.Services;

public class JwtTokenProvider : ITokenProvider
{
    private readonly IOptions<AuthSettings> _authSettings;

    public JwtTokenProvider(IOptions<AuthSettings> authSettings)
    {
        _authSettings = authSettings;
    }

    public string GetToken(ApplicationUser user, List<string> roles)
    {
        var expiration = DateTime.UtcNow.AddMinutes(30);
        var token = CreateJwtToken(CreateClaims(user, roles), CreateSigningCredentials(), expiration);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.IssuerSigningKey)), 
            SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> CreateClaims(ApplicationUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, 
               DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, roles.First())
        };

        return claims;
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
    {
        return new(_authSettings.Value.Issuer, _authSettings.Value.Audience, claims, 
            expires: expiration, signingCredentials: credentials);
    }
}
