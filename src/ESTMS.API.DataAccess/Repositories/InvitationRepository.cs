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

    public async Task<List<Invitation>> GetAllReceivedInvitations(string userId)
    {
        return await _context.Invitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Include(i => i.Team)
            .Where(i => i.Receiver.Id == userId)
            .ToListAsync();
    }

    public async Task<List<Invitation>> GetAllSentInvitations(string userId)
    {
        return await _context.Invitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Include(i => i.Team)
            .Where(i => i.Sender.Id == userId)
            .ToListAsync();
    }

    public async Task<Invitation?> GetInvitationByIdAsync(int id)
    {
        return await _context.Invitations
            .Include(i => i.Sender)
            .Include(i => i.Receiver)
            .Include(i => i.Team)
            .ThenInclude(t => t.Players)
            .Where(i => i.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task UpdateInvitationAsync(Invitation invitation)
    {
        _context.Invitations.Update(invitation);
        await _context.SaveChangesAsync();
    }
}
