using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> GetPlayerByIdAsync(string id)
    {
        return await _context.Players
            .Include(t => t.Team)
            .ThenInclude(tm => tm.TeamManager)
            .ThenInclude(userInfo => userInfo.ApplicationUser)
            .Include(u => u.ApplicationUser)
            .Where(p => p.ApplicationUser.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
        return await _context.Players
            .Include(t => t.Team)
            .ThenInclude(tm => tm.TeamManager)
            .Include(u => u.ApplicationUser)
            .ToListAsync();
    }

    public async Task<Player> UpdatePlayerAsync(Player updatedPlayer)
    {
        _context.Players.Update(updatedPlayer);

        await _context.SaveChangesAsync();

        return updatedPlayer;
    }
}