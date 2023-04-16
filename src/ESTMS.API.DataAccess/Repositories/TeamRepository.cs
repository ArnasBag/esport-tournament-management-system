using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Team> CreateTeamAsync(Team team)
    {
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        return team;        
    }
}
