using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IInvitationRepository
{
    Task<Invitation> CreateInvitationAsync(Invitation invitation);
    Task<Invitation?> GetInvitationByIdAsync(int id, string userId);
    Task UpdateInvitationAsync(Invitation invitation);
    Task<List<Invitation>> GetAllInvitationsAsync(string receiverId);
}
