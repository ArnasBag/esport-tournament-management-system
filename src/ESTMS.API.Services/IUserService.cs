using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IUserService
{
    public Task<ApplicationUser> GetUserByIdAsync(string id);
}
