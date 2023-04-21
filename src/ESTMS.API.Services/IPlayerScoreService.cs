using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IPlayerScoreService
{
    Task<List<PlayerScore>> GetPlayerScoresByMatchIdAsync(int matchId);
}
