using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IUserRepository
{
    Task CreatePlayerAsync(Player player);
    public Task<ApplicationUser?> GetUserByIdAsync(string id);
    public Task<Player?> GetPlayerByUserIdAsync(string userId);
    public Task<TeamManager?> GetTeamManagerByUserIdAsync(string userId);
    public Task<List<ApplicationUser>> GetUsersAsync();
    public Task UpdateUserAsync(ApplicationUser user);
}
