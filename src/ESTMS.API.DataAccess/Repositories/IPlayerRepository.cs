using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerByIdAsync(string id);
    Task<List<Player>> GetAllPlayersAsync();
    Task<Player> UpdatePlayerAsync(Player updatedPlayer);
}