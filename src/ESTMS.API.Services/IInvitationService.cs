using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IInvitationService
{
    Task<Invitation> CreateInviteForTeamAsync(int teamId);
    Task<Invitation> CreateInviteForPlayerAsync(string playerUserId, int teamId);
    Task ChangeInvitationStatusAsync(int id, InvitationStatus status);
    Task<List<Invitation>> GetAllInvitationsAsync(bool sent, bool received);
}
