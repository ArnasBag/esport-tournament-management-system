using ESTMS.API.DataAccess.Data;
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
            .Where(m => m.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task UpdateMatchAsync(Match match)
    {
        _context.Matches.Update(match);
        await _context.SaveChangesAsync();
    }
}
