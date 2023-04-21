using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IPlayerScoreRepository
{
    Task<List<PlayerScore>> GetPlayerScoresByMatchIdAsync(int matchId);

}
