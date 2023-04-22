using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface ITournamentRepository
{
    Task<Tournament?> GetTournamentByIdAsync(int id);
    Task<List<Tournament>> GetAllTournamentsAsync();
    Task<Tournament> CreateTournamentAsync(Tournament tournament);
    Task<Tournament> UpdateTournamentAsync(Tournament updatedTournament);
    Task RemoveTournamentAsync(Tournament tournaments);
}