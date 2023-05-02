using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services.Auth;

public interface ITokenProvider
{
    public string GetToken(ApplicationUser user, List<string> roles);
}
