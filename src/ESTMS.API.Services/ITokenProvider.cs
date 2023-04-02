using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface ITokenProvider
{
    public string GetToken(ApplicationUser user, List<string> roles);
}
