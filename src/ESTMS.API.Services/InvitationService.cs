using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserIdProvider _userIdProvider;

    public InvitationService(IInvitationRepository invitationRepository,
        ITeamRepository teamRepository,
        IUserRepository userRepository,
        IUserIdProvider userIdProvider)
    {
        _invitationRepository = invitationRepository;
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _userIdProvider = userIdProvider;
    }

    public async Task ChangeInvitationStatusAsync(int id, InvitationStatus status)
    {
        var userId = _userIdProvider.UserId ?? throw new Exception();

        var invitation = await _invitationRepository.GetInvitationByIdAsync(id, userId)
            ?? throw new NotFoundException("Invitation with this id was not found.");

        if(invitation.Status != InvitationStatus.Pending)
        {
            throw new BadRequestException("Invitation is already accepted or declined.");
        }

        invitation.Status = status;
        invitation.UpdatedAt = DateTime.UtcNow;

        await _invitationRepository.UpdateInvitationAsync(invitation);

        if (status == InvitationStatus.Accepted)
        {
            var player = await _userRepository.GetPlayerByUserIdAsync(userId);
            await _teamRepository.AssignPlayerToTeamAsync(invitation.Team, player);
        }
    }

    public async Task<Invitation> CreateInvitationAsync(int teamId, string receiverId)
    {
        var team = await _teamRepository.GetTeamByIdAsync(teamId)
            ?? throw new NotFoundException();

        var receiver = await _userRepository.GetPlayerByUserIdAsync(receiverId) 
            ?? throw new NotFoundException();

        if(receiver.Team != null)
        {
            throw new BadRequestException("Player is already part of a team.");
        }

        var teamManagerUserId = _userIdProvider.UserId;

        if(team.TeamManager.ApplicationUser.Id != teamManagerUserId)
        {
            throw new ForbiddenException("Cannot invite player to a team you don't have access to.");
        }

        var invitation = await _invitationRepository.CreateInvitationAsync(new Invitation
        {
            Sender = team.TeamManager,
            Receiver = receiver,
            Team = team,
            Status = InvitationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });

        return invitation;
    }
}
