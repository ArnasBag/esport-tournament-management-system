using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IPlayerService
{
    Task<Player> UpdatePlayerAsync(int id, Player updatedPlayer);
    Task<Player> GetPlayerByIdAsync(int id);
    Task<List<Player>> GetAllPlayersAsync(string? userId = null);
    Task<Player> UpdatePlayersRankAsync(int id);
    Task<Player> UpdatePlayersPointAsync(int id, int points);
}