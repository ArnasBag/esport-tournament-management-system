using ESTMS.API.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace ESTMS.API.Services;

public interface IPlayerService
{
    Task<Player> UpdatePlayerAsync(string id, Player updatedPlayer, IFormFile profilePicture);
    Task<Player> GetPlayerByIdAsync(string id);
    Task<List<Match>> GetPlayerWonMatches(string id);
    Task<List<Player>> GetAllPlayersAsync(string? userId = null);
    Task<Player> UpdatePlayersRankAsync(string id);
    Task<Player> UpdatePlayersPointAsync(string id, int points);
}