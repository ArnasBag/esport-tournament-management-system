using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerByIdAsync(int id);
    Task<List<Player>> GetAllPlayersAsync();
    Task<Player> CreatePlayerAsync(Player player);
    Task<Player> UpdatePlayerAsync(Player updatedPlayer);
    Task<Player> UpdatePlayerRankAsync(Player updatedPlayer);
    Task<Player> UpdatePlayerPointsAsync(Player updatedPlayer);
}