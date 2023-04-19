using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

public class InvitationServiceTests
{
    private Mock<IInvitationRepository> _invitationRepositoryMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserIdProvider> _userIdProviderMock;

    private IInvitationService _invitationService;

    [SetUp]
    public void Setup()
    {
        _invitationRepositoryMock = new Mock<IInvitationRepository>();
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userIdProviderMock = new Mock<IUserIdProvider>();

        _invitationService = new InvitationService(_invitationRepositoryMock.Object, 
            _teamRepositoryMock.Object, _userRepositoryMock.Object, 
            _userIdProviderMock.Object);
    }

    [Test]
    public void ChangeInvitationStatusAsync_InvitationNotFound_ThrowsException()
    {
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(default(Invitation));

        Assert.ThrowsAsync<NotFoundException>(
            () => _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), It.IsAny<InvitationStatus>()));
    }

    [Test]
    public void ChangeInvitationStatusAsync_InvitationAlreadyAccepted_ThrowsException()
    {
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new Invitation { Status = InvitationStatus.Accepted });

        Assert.ThrowsAsync<BadRequestException>(
            () => _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), InvitationStatus.Accepted));
    }

    [Test]
    public async Task ChangeInvitationStatusAsync_DeclinedInvitation_Ok()
    {
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new Invitation { Status = InvitationStatus.Pending });

        await _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), InvitationStatus.Declined);

        _invitationRepositoryMock.Verify(x => x.GetInvitationByIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        _invitationRepositoryMock.Verify(x => x.UpdateInvitationAsync(It.IsAny<Invitation>()), Times.Once);
    }

    [Test]
    public async Task ChangeInvitationStatusAsync_AcceptedInvitation_AddsPlayerToTeam()
    {
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new Invitation { Status = InvitationStatus.Pending });

        await _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), InvitationStatus.Accepted);

        _invitationRepositoryMock.Verify(x => x.GetInvitationByIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        _invitationRepositoryMock.Verify(x => x.UpdateInvitationAsync(It.IsAny<Invitation>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.AssignPlayerToTeamAsync(It.IsAny<Team>(), It.IsAny<Player>()), Times.Once);
    }

    [Test]
    public void CreateInvitationAsync_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(
            () => _invitationService.CreateInvitationAsync(It.IsAny<int>(), It.IsAny<string>()));
    }

    [Test]
    public void CreateInvitationAsync_PlayerNotFound_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(default(Player));

        Assert.ThrowsAsync<NotFoundException>(
            () => _invitationService.CreateInvitationAsync(It.IsAny<int>(), It.IsAny<string>()));
    }

    [Test]
    public void CreateInvitationAsync_PlayerAlreadyInATeam_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team());

        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Player { Team = new Team() });

        Assert.ThrowsAsync<BadRequestException>(
            () => _invitationService.CreateInvitationAsync(It.IsAny<int>(), It.IsAny<string>()));
    }

    [Test]
    public void CreateInvitationAsync_InviteToOtherTeam_ThrowsException()
    {
        string senderTeamManagerUserId = "id";
        string requestedTeamTeamManagerUserId = "notid";

        _userIdProviderMock.Setup(x => x.UserId).Returns(senderTeamManagerUserId);
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Team
            {
                TeamManager = new TeamManager
                {
                    ApplicationUser = new ApplicationUser
                    {
                        Id = requestedTeamTeamManagerUserId
                    }
                }
            });
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Player());

        Assert.ThrowsAsync<ForbiddenException>(
            () => _invitationService.CreateInvitationAsync(It.IsAny<int>(), It.IsAny<string>()));
    }

    [Test]
    public async Task CreateInvitationAsync_Ok()
    {
        string teamManagerUserId = "id";

        _userIdProviderMock.Setup(x => x.UserId).Returns(teamManagerUserId);
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Team
            {
                TeamManager = new TeamManager
                {
                    ApplicationUser = new ApplicationUser
                    {
                        Id = teamManagerUserId
                    }
                }
            });
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Player());

        await _invitationService.CreateInvitationAsync(It.IsAny<int>(), It.IsAny<string>());

        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()), Times.Once);
        _invitationRepositoryMock.Verify(x => x.CreateInvitationAsync(It.IsAny<Invitation>()), Times.Once);
    }
}

