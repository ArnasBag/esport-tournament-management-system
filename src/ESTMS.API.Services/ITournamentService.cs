using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface ITournamentService
{
    Task<List<Tournament>> GetAllTournamentsAsync();
    Task<Tournament> GetTournamentByIdAsync(int id);
    Task<Tournament> CreateTournamentAsync(Tournament tournament);
    Task<Tournament> UpdateTournamentAsync(int id, Tournament updatedTournament);
    Task<Tournament> UpdateTournamentWinnerAsync(int id, TournamentWinner updatedWinner);
    Task<Tournament> UpdateTournamentStatusAsync(int id, Status updatedStatus);
    Task<Tournament> GetTournamentByTournamentManagerId(string id);
}