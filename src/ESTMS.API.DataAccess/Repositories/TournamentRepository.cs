﻿using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Repositories;

public class TournamentRepository : ITournamentRepository
{
    private readonly ApplicationDbContext _context;

    public TournamentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Tournament?> GetTournamentByIdAsync(int id)
    {
        return await _context.Tournaments
            .Include(w => w.Winner)
            .ThenInclude(t => t.WinnerTeam)
            .Include(t => t.Teams)
            .ThenInclude(m => m.TeamManager)
            .Include(m => m.Matches)
            .ThenInclude(c => c.Competitors)
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Tournament>> GetAllTournamentsAsync()
    {
        return await _context.Tournaments
            .Include(w => w.Winner)
            .ThenInclude(t => t.WinnerTeam)
            .Include(t => t.Teams)
            .ThenInclude(m => m.TeamManager)
            .Include(m => m.Matches)
            .ThenInclude(c => c.Competitors)
            .ToListAsync();
    }

    public async Task<Tournament> CreateTournamentAsync(Tournament tournament)
    {
        await _context.Tournaments.AddAsync(tournament);
        await _context.SaveChangesAsync();

        return tournament;
    }

    public async Task<Tournament> UpdateTournamentAsync(Tournament updatedTournament)
    {
        _context.Tournaments.Update(updatedTournament);

        await _context.SaveChangesAsync();

        return updatedTournament;
    }

    public async Task RemoveTournamentAsync(Tournament tournaments)
    {
        _context.Tournaments.Remove(tournaments);
        await _context.SaveChangesAsync();
    }
}