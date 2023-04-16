using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

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
