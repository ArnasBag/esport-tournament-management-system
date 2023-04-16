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

    public async Task<Team?> GetTeamByIdAsync(int id)
    {
        return await _context.Teams.FindAsync(id);
    }

    public async Task<Team> UpdateTeamAsync(Team updatedTeam)
    {
        _context.Teams.Update(updatedTeam);

        await _context.SaveChangesAsync();

        return updatedTeam;
    }
}
