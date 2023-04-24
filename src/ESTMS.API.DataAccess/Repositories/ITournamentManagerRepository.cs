using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface ITournamentManagerRepository
{
    Task CreateAsync(TournamentManager tournamentManager);
    Task RemoveAsync(TournamentManager tournamentManager);
}