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
            .ThenInclude(x => x.WinnerTeamId)
            .Where(r => r.Id == roundId)
            .SingleOrDefaultAsync();
    }
}