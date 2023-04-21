using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IInvitationRepository
{
    Task<Invitation> CreateInvitationAsync(Invitation invitation);
    Task<Invitation?> GetInvitationByIdAsync(int id);
    Task UpdateInvitationAsync(Invitation invitation);
    Task<List<Invitation>> GetAllReceivedInvitations(string playerUserId);
    Task<List<Invitation>> GetAllSentInvitations(string teamManagerUserId);

}
