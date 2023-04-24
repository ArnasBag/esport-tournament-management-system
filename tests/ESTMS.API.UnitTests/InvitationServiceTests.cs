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
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Invitation));

        Assert.ThrowsAsync<NotFoundException>(
            () => _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), It.IsAny<InvitationStatus>()));
    }

    [Test]
    public void ChangeInvitationStatusAsync_InvitationAlreadyAccepted_ThrowsException()
    {
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Invitation { Status = InvitationStatus.Accepted });

        Assert.ThrowsAsync<BadRequestException>(
            () => _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), InvitationStatus.Accepted));
    }

    [Test]
    public void ChangeInvitationStatus_SenderTriesToAccept_ThrowsException()
    {
        string receiverUserId = "a";
        string userIdWhoAccepts = "b";

        _userIdProviderMock.Setup(x => x.UserId).Returns(userIdWhoAccepts);
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Invitation
            {
                Receiver = new ApplicationUser
                {
                    Id = receiverUserId
                }
            });

        Assert.ThrowsAsync<BadRequestException>(
            () => _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), InvitationStatus.Accepted));
    }

    [Test]
    public async Task ChangeInvitationStatus_TeamManagerAcceptsInvitation_Ok()
    {
        string receiverUserId = "a";
        string userIdWhoAccepts = "a";

        _userIdProviderMock.Setup(x => x.UserId).Returns(userIdWhoAccepts);
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Invitation
            {
                Receiver = new ApplicationUser
                {
                    Id = receiverUserId
                },
                Status = InvitationStatus.Pending,
                Sender = new ApplicationUser()
            });
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(default(Player));

        await _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), InvitationStatus.Accepted);

        _invitationRepositoryMock.Verify(x => x.GetInvitationByIdAsync(It.IsAny<int>()), Times.Once);
        _invitationRepositoryMock.Verify(x => x.UpdateInvitationAsync(It.IsAny<Invitation>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()), Times.Exactly(2));
        _teamRepositoryMock.Verify(x => x.AssignPlayerToTeamAsync(It.IsAny<Team>(), It.IsAny<Player>()), Times.Once);
    }

    [Test]
    public async Task ChangeInvitationStatus_PlayerAcceptsInvitation_Ok()
    {
        string receiverUserId = "a";
        string userIdWhoAccepts = "a";

        _userIdProviderMock.Setup(x => x.UserId).Returns(userIdWhoAccepts);
        _invitationRepositoryMock.Setup(x => x.GetInvitationByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Invitation
            {
                Receiver = new ApplicationUser
                {
                    Id = receiverUserId
                },
                Status = InvitationStatus.Pending,
                Sender = new ApplicationUser()
            });
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player());

        await _invitationService.ChangeInvitationStatusAsync(It.IsAny<int>(), InvitationStatus.Accepted);

        _invitationRepositoryMock.Verify(x => x.GetInvitationByIdAsync(It.IsAny<int>()), Times.Once);
        _invitationRepositoryMock.Verify(x => x.UpdateInvitationAsync(It.IsAny<Invitation>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.AssignPlayerToTeamAsync(It.IsAny<Team>(), It.IsAny<Player>()), Times.Once);
    }

    [Test]
    public void CreateInviteForTeamAsync_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(
            () => _invitationService.CreateInviteForTeamAsync(It.IsAny<int>()));
    }

    [Test]
    public void CreateInviteForTeamAsync_SenderAlreadyPartOfTheTeam_ThrowsException()
    {
        int teamId = 1;

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Id = teamId,
            TeamManager = new TeamManager
            {
                ApplicationUser = new ApplicationUser()
            }
        });
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player
        {
            Team = new Team
            {
                Id = teamId
            }
        });
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

        Assert.ThrowsAsync<BadRequestException>(
            () => _invitationService.CreateInviteForTeamAsync(It.IsAny<int>()));
    }

    [Test]
    public void CreateInviteForTeamAsync_SenderAlreadyPartOtherTeam_ThrowsException()
    {
        int teamId = 1;
        int otherTeamId = 2;

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Id = teamId,
            TeamManager = new TeamManager
            {
                ApplicationUser = new ApplicationUser()
            }
        });
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player
        {
            Team = new Team
            {
                Id = otherTeamId
            }
        });
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

        Assert.ThrowsAsync<BadRequestException>(
            () => _invitationService.CreateInviteForTeamAsync(It.IsAny<int>()));
    }

    [Test]
    public async Task CreateInviteForTeamAsync_ValidData_Ok()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Id = 1,
            TeamManager = new TeamManager
            {
                ApplicationUser = new ApplicationUser()
            }
        });
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player());
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

        await _invitationService.CreateInviteForTeamAsync(It.IsAny<int>());

        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Exactly(2));
        _userRepositoryMock.Verify(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()), Times.Once);
        _invitationRepositoryMock.Verify(x => x.CreateInvitationAsync(It.IsAny<Invitation>()), Times.Once);

    }

    [Test]
    public void CreateInviteForPlayerAsync_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(
            () => _invitationService.CreateInviteForPlayerAsync(It.IsAny<string>(), It.IsAny<int>()));
    }

    [Test]
    public void CreateInviteForPlayerAsync_InviteToTeamWithNoAccess_ThrowsException()
    {
        string teamManagerUserId = "a";
        string teamTeamManagerUserId = "b";

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            TeamManager = new TeamManager
            {
                ApplicationUser = new ApplicationUser
                {
                    Id = teamTeamManagerUserId
                }
            }
        });
        _userIdProviderMock.Setup(x => x.UserId).Returns(teamManagerUserId);

        Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _invitationService.CreateInviteForPlayerAsync(It.IsAny<string>(), It.IsAny<int>()));
    }

    [Test]
    public void CreateInviteForPlayerAsync_PlayerNotFound_ThrowsException()
    {
        string teamManagerUserId = "a";
        string teamTeamManagerUserId = "a";

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            TeamManager = new TeamManager
            {
                ApplicationUser = new ApplicationUser
                {
                    Id = teamTeamManagerUserId
                }
            }
        });
        _userIdProviderMock.Setup(x => x.UserId).Returns(teamManagerUserId);
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(default(ApplicationUser));

        Assert.ThrowsAsync<NotFoundException>(
            () => _invitationService.CreateInviteForPlayerAsync(It.IsAny<string>(), It.IsAny<int>()));
    }

    [Test]
    public void CreateInviteForPlayerAsync_PlayerAlreadyBelongsToATeam_ThrowsException()
    {
        string teamManagerUserId = "a";
        string teamTeamManagerUserId = "a";

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            TeamManager = new TeamManager
            {
                ApplicationUser = new ApplicationUser
                {
                    Id = teamTeamManagerUserId
                }
            }
        });
        _userIdProviderMock.Setup(x => x.UserId).Returns(teamManagerUserId);
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player
        {
            Team = new Team()
        });

        Assert.ThrowsAsync<BadRequestException>(
            () => _invitationService.CreateInviteForPlayerAsync(It.IsAny<string>(), It.IsAny<int>()));
    }

    [Test]
    public async Task CreateInviteForPlayerAsync_ValidData_Ok()
    {
        string teamManagerUserId = "a";
        string teamTeamManagerUserId = "a";

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            TeamManager = new TeamManager
            {
                ApplicationUser = new ApplicationUser
                {
                    Id = teamTeamManagerUserId
                }
            }
        });
        _userIdProviderMock.Setup(x => x.UserId).Returns(teamManagerUserId);
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player());

        await _invitationService.CreateInviteForPlayerAsync(It.IsAny<string>(), It.IsAny<int>());

        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()), Times.Once);
        _invitationRepositoryMock.Verify(x => x.CreateInvitationAsync(It.IsAny<Invitation>()), Times.Once);
    }

    [Test]
    public void GetAllInvitationsAsync_BothFiltersFalse_ThrowsException()
    {
        Assert.ThrowsAsync<BadRequestException>(
            () => _invitationService.GetAllInvitationsAsync(false, false));
    }

    [Test]
    public async Task GetAllInvitationsAsync_SentTrueCallsGetAllSentInvitations_ThrowsException()
    {
        await _invitationService.GetAllInvitationsAsync(true, false);

        _invitationRepositoryMock.Verify(x => x.GetAllSentInvitations(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetAllInvitationsAsync_ReceivedTrueCallsGetAllReceivedInvitations_ThrowsException()
    {
        await _invitationService.GetAllInvitationsAsync(false, true);

        _invitationRepositoryMock.Verify(x => x.GetAllReceivedInvitations(It.IsAny<string>()), Times.Once);
    }
}

