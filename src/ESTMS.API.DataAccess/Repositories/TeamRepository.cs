using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AssignPlayerToTeamAsync(Team team, Player player)
    {
        team.Players.Add(player);
        await _context.SaveChangesAsync();
    }

    public async Task<Team> CreateTeamAsync(Team team)
    {
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        return team;        
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        return await _context.Teams.ToListAsync();
    }

    public async Task<Team?> GetTeamByIdAsync(int id)
    {
        return await _context.Teams.Include(t => t.TeamManager)
            .ThenInclude(m => m.ApplicationUser)
            .Where(t => t.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<Team> UpdateTeamAsync(Team updatedTeam)
    {
        _context.Teams.Update(updatedTeam);

        await _context.SaveChangesAsync();

        return updatedTeam;
    }
}
