﻿using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly ApplicationDbContext _context;

    public MatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Match?> GetMatchByIdAsync(int id)
    {
        return await _context.Matches
            .Include(m => m.PlayerScores)
            .Include(m => m.Competitors)
            .ThenInclude(m => m.Players)
            .ThenInclude(p => p.ApplicationUser)
            .Include(m => m.Winner)
            .Include(r => r.Round)
            .Where(m => m.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<Match> UpdateMatchAsync(Match match)
    {
        _context.Matches.Update(match);

        await _context.SaveChangesAsync();

        return match;
    }

    public async Task<List<Match>> GetPlayerWonMatchesAsync(string id)
    {
        return await _context.Matches
            .Include(m => m.Winner)
            .ThenInclude(w => w.WinnerTeam)
            .Where(m => m.Winner.WinnerTeam.Players.Any(p => p.ApplicationUser.Id == id))
            .ToListAsync();
    }
}