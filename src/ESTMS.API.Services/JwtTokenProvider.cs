using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ESTMS.API.Services;

public class JwtTokenProvider : ITokenProvider
{
    public string GetToken(string userId)
    {
        var expiration = DateTime.UtcNow.AddMinutes(30);
        var token = CreateJwtToken(CreateClaims(userId), CreateSigningCredentials(), expiration);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("randomrandomrandomrandomrandomrandomrandomrandomrandomrandom")), 
            SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> CreateClaims(string userId)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, 
               DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.NameIdentifier, userId),
        };

        return claims;
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
    {
        return new("estms", "estms", claims, 
            expires: expiration, signingCredentials: credentials);
    }
}
