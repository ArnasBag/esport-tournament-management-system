using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IMatchRepository
{
    Task<Match> UpdateMatchAsync(Match match);
    Task<Match?> GetMatchByIdAsync(int id);
    Task<List<Match>> GetPlayerWonMatchesAsync(string id);
}
