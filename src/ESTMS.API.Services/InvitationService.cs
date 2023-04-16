using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;

    public InvitationService(IInvitationRepository invitationRepository, 
        ITeamRepository teamRepository, 
        IUserRepository userRepository)
    {
        _invitationRepository = invitationRepository;
        _teamRepository = teamRepository;
        _userRepository = userRepository;
    }

    public async Task<Invitation> CreateInvitationAsync(int teamId, string receiverId)
    {
        var team = await _teamRepository.GetTeamByIdAsync(teamId)
            ?? throw new NotFoundException();

        var receiver = await _userRepository.GetPlayerByUserId(receiverId) 
            ?? throw new NotFoundException();

        //if(receiver.Invitations.Where(i => i.Sender == senderId).Any(i => i.Status == pending))

        var invitation = await _invitationRepository.CreateInvitationAsync(new Invitation
        {
            Sender = new TeamManager(),
            Receiver = receiver,
            Team = team,
            Status = InvitationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });

        return invitation;
    }
}
