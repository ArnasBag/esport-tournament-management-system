using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<ApplicationUser>> GetUsersAsync()
    {
        return await _context.Users.OrderBy(x => x.Id).ToListAsync();
    }

    public async Task UpdateUserAsync(ApplicationUser user)
    {
        _context.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task CreatePlayerAsync(Player player)
    {
        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();
    }

    public async Task<Player?> GetPlayerByUserIdAsync(string userId)
    {
        return await _context.Players
            .Include(p => p.ApplicationUser)
            .ThenInclude(u => u.ReceivedInvitations)
            .ThenInclude(i => i.Sender)
            .Include(p => p.Scores)
            .SingleOrDefaultAsync(p => p.ApplicationUser.Id == userId);
    }

    public async Task<TeamManager?> GetTeamManagerByUserIdAsync(string userId)
    {
        return await _context.TeamManagers
            .Include(t => t.ApplicationUser)
            .Include(t => t.Teams)
            .SingleOrDefaultAsync(t => t.ApplicationUser.Id == userId);
    }

    public async Task<TournamentManager?> GetTournamentManagerByUserIdAsync(string userId)
    {
        return await _context.TournamentManagers
            .Include(t => t.ApplicationUser)
            .Include(t => t.Tournaments)
            .SingleOrDefaultAsync(t => t.ApplicationUser.Id == userId);
    }
}