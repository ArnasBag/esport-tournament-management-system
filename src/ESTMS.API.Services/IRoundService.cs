﻿using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IRoundService
{
    Task<Round> GetRoundByIdAsync(int id);
    Task<List<Round>> GetTournamentRounds(int tournamentId);
}