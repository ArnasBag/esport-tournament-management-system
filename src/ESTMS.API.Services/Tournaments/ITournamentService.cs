﻿using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services.Tournaments;

public interface ITournamentService
{
    Task<List<Tournament>> GetAllTournamentsAsync(string? userId = null);
    Task<Tournament> GetTournamentByIdAsync(int id);
    Task<Tournament> CreateTournamentAsync(Tournament tournament);
    Task<Tournament> UpdateTournamentAsync(int id, Tournament updatedTournament);
    Task<Tournament> UpdateTournamentWinnerAsync(int id, int updatedWinner);
    Task<Tournament> UpdateTournamentStatusAsync(int id, Status updatedStatus);
    Task<Tournament> GetTournamentByTournamentManagerId(string id);
    Task JoinTournamentAsync(int tournamentId, int teamId);
    Task LeaveTournamentAsync(int tournamentId, int teamId);
    Task<Tournament> CreateBracket(int id);
    Task<Tournament> UpdateTournamentBracket(int roundId);
}