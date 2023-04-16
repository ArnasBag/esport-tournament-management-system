using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly ApplicationDbContext _context;

    public InvitationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Invitation> CreateInvitationAsync(Invitation invitation)
    {
        await _context.Invitations.AddAsync(invitation);

        await _context.SaveChangesAsync();

        return invitation;
    }

    public async Task<List<Invitation>> GetAllInvitationsAsync(string receiverId)
    {
        return await _context.Invitations
            .Include(i => i.Receiver)
            .ThenInclude(r => r.ApplicationUser)
            .Include(i => i.Sender)
            .ThenInclude(s => s.ApplicationUser)
            .Include(i => i.Team)
            .Where(i => i.Receiver.ApplicationUser.Id == receiverId)
            .ToListAsync();
    }

    public async Task<Invitation?> GetInvitationByIdAsync(int id, string userId)
    {
        return await _context.Invitations.Include(i => i.Team)
            .ThenInclude(t => t.Players)
            .Where(i => i.Id == id)
            .Where(i => i.Receiver.ApplicationUser.Id == userId)
            .SingleOrDefaultAsync();
    }

    public async Task UpdateInvitationAsync(Invitation invitation)
    {
        _context.Invitations.Update(invitation);
        await _context.SaveChangesAsync();
    }
}
