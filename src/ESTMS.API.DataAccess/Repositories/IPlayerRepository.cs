using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerByIdAsync(int id);
    Task<List<Player>> GetAllPlayersAsync();
    Task<Player> UpdatePlayerAsync(Player updatedPlayer);
}