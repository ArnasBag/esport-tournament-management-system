using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services.Users;

public interface IUserService
{
    public Task<ApplicationUser> GetUserByIdAsync(string id);
    public Task<List<ApplicationUserWithRole>> GetUsersAsync();
    public Task<List<DailyUserCreatedCount>> GetDailyCreatedUsersAsync(DateTime from, DateTime to);
    public Task ChangeUserActivityAsync(string id, bool status);
    public Task ChangeUserRoleAsync(string id, string role);

}
