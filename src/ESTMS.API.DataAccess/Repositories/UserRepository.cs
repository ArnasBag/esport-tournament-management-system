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
        return await _context.Users.ToListAsync();
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

    public async Task<Player?> GetPlayerByUserId(string userId)
    {
        return await _context.Players
            .Include(p => p.ApplicationUser)
            .SingleOrDefaultAsync(p => p.ApplicationUser.Id == userId);
    }
}
