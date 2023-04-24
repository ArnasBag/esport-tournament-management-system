using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IMatchRepository
{
    Task UpdateMatchAsync(Match match);
    Task<Match?> GetMatchByIdAsync(int id);
}
