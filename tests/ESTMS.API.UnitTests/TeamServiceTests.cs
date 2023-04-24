using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

public class TeamServiceTests
{
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserIdProvider> _userIdProviderMock;

    private ITeamService _teamService;

    [SetUp]
    public void Setup()
    {
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userIdProviderMock = new Mock<IUserIdProvider>();

        _teamService = new TeamService(_teamRepositoryMock.Object, _userRepositoryMock.Object, _userIdProviderMock.Object);
    }

    [Test]
    public async Task CreateTeamAsync_Ok()
    {
        _userRepositoryMock.Setup(x => x.GetTeamManagerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new TeamManager());

        await _teamService.CreateTeamAsync(new Team());

        _userRepositoryMock.Verify(x => x.GetTeamManagerByUserIdAsync(It.IsAny<string>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.CreateTeamAsync(It.IsAny<Team>()), Times.Once);
    }

    [Test]
    public async Task UpdateTeamAsync_ValidData_Ok()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team());

        await _teamService.UpdateTeamAsync(It.IsAny<int>(), new Team());

        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.UpdateTeamAsync(It.IsAny<Team>()), Times.Once);
    }

    [Test]
    public void UpdateTeamAsync_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(() => _teamService.UpdateTeamAsync(It.IsAny<int>(), new Team()));
    }

    [Test]
    public void DeactivateTeamAsync_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(() => _teamService.DeactivateTeamAsync(It.IsAny<int>()));
    }

    [Test]
    public async Task DeactivateTeamAsync_ValidData_Ok()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team());

        await _teamService.DeactivateTeamAsync(It.IsAny<int>());

        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.UpdateTeamAsync(It.IsAny<Team>()), Times.Once);
    }

    [Test]
    public void GetTeamByIdAsync_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(() => _teamService.GetTeamByIdAsync(It.IsAny<int>()));
    }

    [Test]
    public async Task GetTeamByIdAsync_ValidData_Ok()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team());

        await _teamService.GetTeamByIdAsync(It.IsAny<int>());

        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task GetAllTeamsAsync_TeamManagerUserIdNull_GetsAllTeams()
    {
        string? nullUserId = null;

        await _teamService.GetAllTeamsAsync(nullUserId);

        _teamRepositoryMock.Verify(x => x.GetAllTeamsAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllTeamsAsync_TeamManagerUserIdNotNull_GetsAllTeamManagerTeams()
    {
        string nonNullUserId = "id";

        _userRepositoryMock.Setup(x => x.GetTeamManagerByUserIdAsync(nonNullUserId)).ReturnsAsync(new TeamManager());

        await _teamService.GetAllTeamsAsync(nonNullUserId);

        _userRepositoryMock.Verify(x => x.GetTeamManagerByUserIdAsync(nonNullUserId), Times.Once);
    }

    [Test]
    public void GetAllTeamsAsync_NonExistantTeamManagerId_ThrowsException()
    {
        string nonNullUserId = "id";

        _userRepositoryMock.Setup(x => x.GetTeamManagerByUserIdAsync(nonNullUserId)).ReturnsAsync(default(TeamManager));

        Assert.ThrowsAsync<NotFoundException>(() => _teamService.GetAllTeamsAsync(nonNullUserId));
    }
}
