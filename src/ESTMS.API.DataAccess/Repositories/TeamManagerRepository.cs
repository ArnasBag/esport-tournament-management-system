using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public class TeamManagerRepository : ITeamManagerRepository
{
    private readonly ApplicationDbContext _context;

    public TeamManagerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(TeamManager teamManager)
    {
        _context.TeamManagers.Add(teamManager);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(TeamManager teamManager)
    {
        _context.TeamManagers.Remove(teamManager);
        await _context.SaveChangesAsync();
    }
}
