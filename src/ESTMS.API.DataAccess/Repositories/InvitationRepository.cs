using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;

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

    public async Task<Invitation?> GetInvitationByIdAsync(int id)
    {
        return await _context.Invitations.FindAsync(id);
    }
}
