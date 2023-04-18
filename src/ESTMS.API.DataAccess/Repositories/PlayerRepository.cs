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

    public async Task<Player?> GetPlayerByIdAsync(int id)
    {
        return await _context.Players.Include(i => i.Invitations)
            .Include(t => t.Team)
            .Include(u => u.ApplicationUser)
            .Where(p => p.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
        return await _context.Players
            .Include(i => i.Invitations)
            .Include(t => t.Team)
            .Include(u => u.ApplicationUser)
            .ToListAsync();
    }

    public async Task<Player> CreatePlayerAsync(Player player)
    {
        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();

        return player;
    }

    public async Task<Player> UpdatePlayerAsync(Player updatedPlayer)
    {
        _context.Players.Update(updatedPlayer);

        await _context.SaveChangesAsync();

        return updatedPlayer;
    }

    public Task<Player> UpdatePlayerPointsAsync(Player updatedPlayer)
    {
        throw new NotImplementedException();
    }
}