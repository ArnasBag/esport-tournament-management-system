using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IUserService
{
    public Task<ApplicationUser> GetUserByIdAsync(string id);
    public Task<List<ApplicationUser>> GetUsersAsync();
    public Task ChangeUserActivityAsync(string id, bool status);
    public Task ChangeUserRoleAsync(string id, string role);

}
