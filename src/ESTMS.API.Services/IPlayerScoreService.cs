using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IPlayerScoreService
{
    Task<List<PlayerScore>> GetPlayerScoresByMatchIdAsync(int matchId);
    Task<List<PlayerScore>> GetPlayerScoresByUserId(string userId);
    Task<PlayerScore> CreatePlayerScoreAsync(string userId, int matchId, PlayerScore playerScore);
    Task<double> GetPlayerKdaAsync(string userId);
}
