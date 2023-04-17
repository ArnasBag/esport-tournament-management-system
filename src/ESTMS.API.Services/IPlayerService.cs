using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IPlayerService
{
    Task<Player> CreatePlayerAsync(Player player);
    Task<Player> UpdatePlayerAsync(int id, Player updatedPlayer);
    Task<Player> GetPlayerByIdAsync(int id);
    Task<List<Player>> GetAllPlayersAsync(string? userId = null);
}