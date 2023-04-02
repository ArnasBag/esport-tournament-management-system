using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IUserRepository
{
    public Task<ApplicationUser?> GetUserByIdAsync(string id);
}
