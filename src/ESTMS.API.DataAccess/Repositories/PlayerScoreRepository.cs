using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ESTMS.API.DataAccess.Repositories;

public class PlayerScoreRepository : IPlayerScoreRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerScoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AssignPlayerScoreToPlayerAsync(Player player, PlayerScore playerScore)
    {
        player.Scores.Add(playerScore);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PlayerScore>> GetPlayerScoresByMatchIdAsync(int matchId)
    {
        return await _context.PlayerScores
            .Include(p => p.Match)
            .Where(p => p.Match.Id == matchId)
            .ToListAsync();
    }

    public async Task<List<PlayerScore>> GetPlayerScoresByUserId(string userId)
    {
        return await _context.PlayerScores
            .Include(p => p.Player)
            .ThenInclude(p => p.ApplicationUser)
            .Include(p => p.Match)
            .Where(p => p.Player.ApplicationUser.Id == userId)
            .ToListAsync();
    }
}
