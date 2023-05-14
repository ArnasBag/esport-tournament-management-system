using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Repositories;

public class RoundRepository : IRoundRepository
{
    private readonly ApplicationDbContext _context;

    public RoundRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Round?> GetRoundByIdAsync(int roundId)
    {
        return await _context.Rounds
            .Include(t => t.Tournament)
            .Include(m => m.Matches)
            .ThenInclude(c => c.Competitors)
            .Include(m => m.Matches)
            .ThenInclude(w => w.Winner)
            .Where(r => r.Id == roundId)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Round>> GetRoundsByTournamentId(int tournamentId)
    {
        return await _context.Rounds
            .Include(t => t.Tournament)
            .Include(m => m.Matches)
            .ThenInclude(c => c.Competitors)
            .Include(m => m.Matches)
            .ThenInclude(w => w.Winner)
            .Where(r => r.Tournament.Id == tournamentId)
            .ToListAsync();
    }
}