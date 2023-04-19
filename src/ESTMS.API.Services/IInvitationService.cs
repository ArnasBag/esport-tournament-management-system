using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IInvitationService
{
    Task<Invitation> CreateInvitationAsync(int teamId, string receiverId);
    Task ChangeInvitationStatusAsync(int id, InvitationStatus status);
    Task<List<Invitation>> GetAllInitationsAsync();
}
