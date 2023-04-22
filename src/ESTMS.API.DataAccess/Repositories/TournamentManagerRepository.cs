using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public class TournamentManagerRepository : ITournamentManagerRepository
{
    private readonly ApplicationDbContext _context;

    public TournamentManagerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(TournamentManager tournamentManager)
    {
        _context.TournamentManagers.Add(tournamentManager);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(TournamentManager tournamentManager)
    {
        _context.TournamentManagers.Remove(tournamentManager);
        await _context.SaveChangesAsync();
    }
}