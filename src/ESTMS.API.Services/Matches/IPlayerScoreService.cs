using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services.Matches;

public interface IPlayerScoreService
{
    Task<List<PlayerScore>> GetPlayerScoresByMatchIdAsync(int matchId);
    Task<List<DailyPlayerScore>> GetPlayerScoresByUserId(string userId, DateTime from, DateTime to);
    Task<List<DailyPlayerScore>> GetPlayerScoresByTeamId(int teamId, DateTime from, DateTime to);
    Task<PlayerScore> CreatePlayerScoreAsync(string userId, int matchId, PlayerScore playerScore);
    Task<double> GetPlayerKdaAsync(string userId);
    Task<double> GetTeamKdaAsync(int id);
}
