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
        var invitation = await _invitationRepository.GetInvitationByIdAsync(id)
            ?? throw new NotFoundException("Invitation with this id was not found.");

        if(invitation.Status == InvitationStatus.Accepted)
        {
            throw new BadRequestException("Invitation is already accepted");
        }

        if(invitation.Receiver.Id != _userIdProvider.UserId!)
        {
            throw new BadRequestException("You did not receive this invitation");
        }

        invitation.Status = status;
        invitation.UpdatedAt = DateTime.UtcNow;

        await _invitationRepository.UpdateInvitationAsync(invitation);

        if (status == InvitationStatus.Accepted)
        {
            var player = await _userRepository.GetPlayerByUserIdAsync(_userIdProvider.UserId!);

            //team manager is the one who accepts invitation
            if(player == null)
            {
                var playerToAssign = await _userRepository.GetPlayerByUserIdAsync(invitation.Sender.Id);
                await _teamRepository.AssignPlayerToTeamAsync(invitation.Team, playerToAssign!);
            }
            //player is the one who accepts invitation
            else
            {
                await _teamRepository.AssignPlayerToTeamAsync(invitation.Team, player!);
            }
        }
    }

    public async Task<Invitation> CreateInviteForTeamAsync(int teamId)
    {
        var team = await _teamRepository.GetTeamByIdAsync(teamId)
            ?? throw new NotFoundException("Team with this if was not found.");

        var senderPlayerUser = await _userRepository.GetUserByIdAsync(_userIdProvider.UserId!);
        var receiverTeamManagerUser = await _userRepository.GetUserByIdAsync(team.TeamManager.ApplicationUser.Id);

        var senderPlayer = await _userRepository.GetPlayerByUserIdAsync(senderPlayerUser.Id);

        if(senderPlayer.Team != null)
        {
            if (senderPlayer.Team.Id == team.Id)
            {
                throw new BadRequestException("You are already part of this team.");
            }

            throw new BadRequestException("You are already part of a team.");
        }

        var invitation = await _invitationRepository.CreateInvitationAsync(new Invitation
        {
            Sender = senderPlayerUser!,
            Receiver = receiverTeamManagerUser!,
            Team = team,
            Status = InvitationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });

        return invitation;
    }

    public async Task<Invitation> CreateInviteForPlayerAsync(string playerUserId, int teamId)
    {
        var team = await _teamRepository.GetTeamByIdAsync(teamId)
            ?? throw new NotFoundException("Team with this id was not found.");

        if (team.TeamManager.ApplicationUser.Id != _userIdProvider.UserId!)
        {
            throw new UnauthorizedAccessException("Cannot invite players to a team you do not manage.");
        }

        var playerUser = await _userRepository.GetUserByIdAsync(playerUserId)
            ?? throw new NotFoundException("Player with this id was not found.");

        var player = await _userRepository.GetPlayerByUserIdAsync(playerUserId);

        if(player!.Team != null)
        {
            throw new BadRequestException("Player with this id is already part of a team.");
        }

        if(player.ApplicationUser.ReceivedInvitations.Any(x => x.Sender.Id == team.TeamManager.ApplicationUser.Id))
        {
            throw new BadRequestException("You already sent an invitation to this player.");
        }

        var invitation = await _invitationRepository.CreateInvitationAsync(new Invitation
        {
            Sender = team.TeamManager.ApplicationUser,
            Receiver = playerUser,
            Team = team,
            Status = InvitationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });

        return invitation;
    }

    public async Task<List<Invitation>> GetAllInvitationsAsync(bool sent, bool received)
    {
        if (sent)
        {
            return await _invitationRepository.GetAllSentInvitations(_userIdProvider.UserId!);
        }
        else if (received)
        {
            return await _invitationRepository.GetAllReceivedInvitations(_userIdProvider.UserId!);

        }
        else
        {
            throw new BadRequestException("You must select either sent or received invitations.");
        }
    }
}
