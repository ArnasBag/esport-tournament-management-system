using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

public class TeamServiceTests
{
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserIdProvider> _userIdProviderMock;
    private Mock<IFileUploader> _fileUploaderMock;
    private Mock<IMmrCalculator> _mmrCalculatorMock;
    private Mock<IPlayerRepository> _playerRepositoryMock;

    private ITeamService _teamService;

    [SetUp]
    public void Setup()
    {
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userIdProviderMock = new Mock<IUserIdProvider>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _mmrCalculatorMock = new Mock<IMmrCalculator>();
        _playerRepositoryMock = new Mock<IPlayerRepository>();

        _teamService = new TeamService(_teamRepositoryMock.Object, _userRepositoryMock.Object, 
            _userIdProviderMock.Object, _fileUploaderMock.Object, _mmrCalculatorMock.Object, _playerRepositoryMock.Object);
    }

    [Test]
    public async Task CreateTeamAsync_Ok()
    {
        _userRepositoryMock.Setup(x => x.GetTeamManagerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new TeamManager());

        await _teamService.CreateTeamAsync(new Team(), It.IsAny<IFormFile>());

        _userRepositoryMock.Verify(x => x.GetTeamManagerByUserIdAsync(It.IsAny<string>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.CreateTeamAsync(It.IsAny<Team>()), Times.Once);
    }

    [Test]
    public async Task UpdateTeamAsync_ValidData_Ok()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team());

        await _teamService.UpdateTeamAsync(It.IsAny<int>(), new Team(), It.IsAny<IFormFile>());

        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.UpdateTeamAsync(It.IsAny<Team>()), Times.Once);
    }

    [Test]
    public void UpdateTeamAsync_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(() => _teamService.UpdateTeamAsync(It.IsAny<int>(), new Team(), It.IsAny<IFormFile>()));
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

    [Test]
    public async Task UpdateTeamPlayersMmrAsync_ShouldRecalculateMmrForAllPlayersInWinnerAndLoserTeams()
    {
        var matchId = 1;
        var winner = new Team 
        { 
            Id = 1, 
            Players = new List<Player> 
            { 
                new Player 
                { 
                    Id = 1,
                    Mmr = 1000,
                    Scores = new List<PlayerScore> 
                    { 
                        new PlayerScore 
                        { 
                            Match = new DataAccess.Entities.Match 
                            { 
                                Id = matchId 
                            } 
                        }
                    } 
                } 
            } 
        };
        var loser = new Team 
        { 
            Id = 2, 
            Players = new List<Player> 
            {
                new Player { 
                    Id = 2, 
                    Mmr = 900, 
                    Scores = new List<PlayerScore> 
                    { 
                        new PlayerScore 
                        { 
                            Match = new DataAccess.Entities.Match 
                            { 
                                Id = matchId 
                            } 
                        } 
                    } 
                } 
            } 
        };
        _mmrCalculatorMock.Setup(x => x.RecalculatePlayerMmr(
            It.IsAny<Player>(), It.IsAny<int>(), It.IsAny<PlayerScore>(), It.IsAny<int>()))
            .Returns((Player player, int opponentAverageMmr, PlayerScore playerScore, int result) => player.Mmr + 25);

        await _teamService.UpdateTeamPlayersMmrAsync(winner, loser, matchId);

        _playerRepositoryMock.Verify(x => x.UpdatePlayerAsync(It.IsAny<Player>()), Times.Exactly(2));

        Assert.That(winner.Players.Single().Mmr, Is.EqualTo(1025));
        Assert.That(loser.Players.Single().Mmr, Is.EqualTo(925));
    }
}
