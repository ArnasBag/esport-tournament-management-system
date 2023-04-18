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
            .Include(r => r.Rank)
            .Where(p => p.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<Player> CreatePlayerAsync(Player player)
    {
        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();

        return player;
    }

    public Task<Player> UpdatePlayerAsync(Player updatedPlayer)
    {
        throw new NotImplementedException();
    }

    public Task<Player> UpdatePlayerRankAsync(Player updatedPlayer)
    {
        throw new NotImplementedException();
    }

    public Task<Player> UpdatePlayerPointsAsync(Player updatedPlayer)
    {
        throw new NotImplementedException();
    }
}