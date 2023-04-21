using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Repositories;

public class PlayerScoreRepository : IPlayerScoreRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerScoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlayerScore>> GetPlayerScoresByMatchIdAsync(int matchId)
    {
        return await _context.PlayerScores
            .Include(p => p.Match)
            .Where(p => p.Match.Id == matchId)
            .ToListAsync();
    }
}
