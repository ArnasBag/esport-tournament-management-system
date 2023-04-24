using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

public class MatchServiceTests
{
    private Mock<IMatchRepository> _matchRepositoryMock;
    private Mock<IPlayerScoreRepository> _playerScoreRepositoryMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<ITeamService> _teamServiceMock;

    private IMatchService _matchService;

    [SetUp]
    public void Setup()
    {
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _playerScoreRepositoryMock = new Mock<IPlayerScoreRepository>();
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _teamServiceMock = new Mock<ITeamService>();

        _matchService = new MatchService(_matchRepositoryMock.Object, _playerScoreRepositoryMock.Object,
            _teamRepositoryMock.Object, _teamServiceMock.Object);
    }

    [Test]
    public void UpdateMatchStatusAsync_MatchNotFound_ThrowsException()
    {
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(default(DataAccess.Entities.Match));

        Assert.ThrowsAsync<NotFoundException>(() => _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), It.IsAny<Status>()));
    }

    [Test]
    public void UpdateMatchStatusAsync_MatchStatusDoneAndPlayerScoresHaveNotBeenFilled_ThrowsException()
    {
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match
        {
            Competitors = new List<Team>()
            {
                new Team
                {
                    Players = new List<Player>()
                    {
                        new Player(),
                        new Player()
                    }
                }
            }
        });
        _playerScoreRepositoryMock.Setup(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>())).ReturnsAsync(new List<PlayerScore>()
        {
            new PlayerScore()
        });

        Assert.ThrowsAsync<BadRequestException>(() => _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), Status.Done));
    }

    [Test]
    public void UpdateMatchStatusAsync_MatchStatusDoneAndWinnerHasNotBeenSet_ThrowsException()
    {
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match
        {
            Winner = null,
            Competitors = new List<Team>()
            {
                new Team
                {
                    Players = new List<Player>()
                    {
                        new Player(),
                    }
                }
            }
        });
        _playerScoreRepositoryMock.Setup(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>())).ReturnsAsync(new List<PlayerScore>()
        {
            new PlayerScore()
        });

        Assert.ThrowsAsync<BadRequestException>(() => _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), Status.Done));
    }

    [Test]
    public async Task UpdateMatchStatusAsync_ValidData_Ok()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team());
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match
        {
            Winner = new MatchWinner(),
            Competitors = new List<Team>()
            {
                new Team
                {
                    Id = 1,
                    Players = new List<Player>()
                    {
                        new Player(),
                    }
                },
                new Team
                {
                    Id = 0,
                    Players = new List<Player>()
                    {
                        new Player(),
                    }
                }
            }
        });
        _playerScoreRepositoryMock.Setup(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>())).ReturnsAsync(new List<PlayerScore>()
        {
            new PlayerScore(),
            new PlayerScore()
        });

        await _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), Status.Done);

        _matchRepositoryMock.Verify(x => x.GetMatchByIdAsync(It.IsAny<int>()), Times.Once);
        _playerScoreRepositoryMock.Verify(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        _teamServiceMock.Verify(x => x.UpdateTeamPlayersMmrAsync(It.IsAny<Team>(), It.IsAny<Team>(), It.IsAny<int>()), Times.Once);
        _matchRepositoryMock.Verify(x => x.UpdateMatchAsync(It.IsAny<DataAccess.Entities.Match>()), Times.Once);
    }

    [Test]
    public void UpdateMatchWinnerAsync_MatchNotFound_ThrowsException()
    {
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(default(DataAccess.Entities.Match));

        Assert.ThrowsAsync<NotFoundException>(() => _matchService.UpdateMatchWinnerAsync(It.IsAny<int>(), It.IsAny<int>()));
    }

    [Test]
    public void UpdateMatchWinnerAsync_WinningTeamNotFound_ThrowsException()
    {
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match());
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(() => _matchService.UpdateMatchWinnerAsync(It.IsAny<int>(), It.IsAny<int>()));
    }

    [Test]
    public void UpdateMatchWinnerAsync_TeamNotParticipatingInMatch_ThrowsException()
    {
        int teamId = 1;
        int matchTeamId = 2;

        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match
        {
            Competitors = new List<Team>()
            {
                new Team
                {
                    Id = matchTeamId
                }
            }
        });
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Id = teamId
        });

        Assert.ThrowsAsync<BadRequestException>(() => _matchService.UpdateMatchWinnerAsync(It.IsAny<int>(), It.IsAny<int>()));
    }

    [Test]
    public void UpdateMatchWinnerAsync_MatchWinnerAlreadySet_ThrowsException()
    {
        int teamId = 1;

        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match
        {
            Winner = new MatchWinner(),
            Competitors = new List<Team>()
            {
                new Team
                {
                    Id = teamId
                }
            }
        });
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Id = teamId
        });

        Assert.ThrowsAsync<BadRequestException>(() => _matchService.UpdateMatchWinnerAsync(It.IsAny<int>(), It.IsAny<int>()));
    }

    [Test]
    public void UpdateMatchWinnerAsync_MatchStatusIsNotInProgress_ThrowsException()
    {
        int teamId = 1;

        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match
        {
            Winner = null,
            Competitors = new List<Team>()
            {
                new Team
                {
                    Id = teamId
                }
            },
            Status = Status.Done
        });
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Id = teamId
        });

        Assert.ThrowsAsync<BadRequestException>(() => _matchService.UpdateMatchWinnerAsync(It.IsAny<int>(), It.IsAny<int>()));
    }

    [Test]
    public async Task UpdateMatchWinnerAsync_ValidData_Ok()
    {
        int teamId = 1;

        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match
        {
            Winner = null,
            Competitors = new List<Team>()
            {
                new Team
                {
                    Id = teamId
                }
            },
            Status = Status.InProgress
        });
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Id = teamId
        });

        await _matchService.UpdateMatchWinnerAsync(It.IsAny<int>(), It.IsAny<int>());

        _matchRepositoryMock.Verify(x => x.GetMatchByIdAsync(It.IsAny<int>()), Times.Once);
        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(It.IsAny<int>()), Times.Once);
        _matchRepositoryMock.Verify(x => x.UpdateMatchAsync(It.IsAny<DataAccess.Entities.Match>()), Times.Once);
    }
}
