using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using ESTMS.API.Services.Matches;
using Moq;
using NUnit.Framework;
using static System.Formats.Asn1.AsnWriter;

namespace ESTMS.API.UnitTests;

public class PlayerScoreServiceTests
{
    private Mock<IPlayerScoreRepository> _playerScoreRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IMatchRepository> _matchRepositoryMock;
    private Mock<ITeamRepository> _teamRepositoryMock;

    private IPlayerScoreService _playerScoreService;

    [SetUp]
    public void Setup()
    {
        _playerScoreRepositoryMock = new Mock<IPlayerScoreRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _teamRepositoryMock = new Mock<ITeamRepository>();

        _playerScoreService = new PlayerScoreService(_playerScoreRepositoryMock.Object, _userRepositoryMock.Object,
            _matchRepositoryMock.Object, _teamRepositoryMock.Object);
    }

    [Test]
    public void CreatePlayerScoreAsync_PlayerNotFound_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(default(Player));

        Assert.ThrowsAsync<NotFoundException>(() =>
            _playerScoreService.CreatePlayerScoreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PlayerScore>()));
    }

    [Test]
    public void CreatePlayerScoreAsync_MatchNotFound_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player());
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(DataAccess.Entities.Match));

        Assert.ThrowsAsync<NotFoundException>(() =>
            _playerScoreService.CreatePlayerScoreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PlayerScore>()));
    }

    [Test]
    public void CreatePlayerScoreAsync_MatchStatusIsNotInProgress_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player());
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new DataAccess.Entities.Match
            {
                Status = Status.NotStarted
            });

        Assert.ThrowsAsync<BadRequestException>(() =>
            _playerScoreService.CreatePlayerScoreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PlayerScore>()));
    }

    [Test]
    public void CreatePlayerScoreAsync_PlayerNotPartOfMatch_ThrowsException()
    {
        int givenPlayerId = 1;
        int matchPlayerid = 2;

        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player()
        {
            Id = givenPlayerId
        });
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new DataAccess.Entities.Match
            {
                Status = Status.InProgress,
                Competitors = new()
                {
                    new Team
                    {
                        Players = new()
                        {
                            new Player { Id = matchPlayerid }
                        }
                    }
                }
            });

        Assert.ThrowsAsync<BadRequestException>(() =>
            _playerScoreService.CreatePlayerScoreAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<PlayerScore>()));
    }

    [Test]
    public void CreatePlayerScoreAsync_PlayerScoreHasNegativeValues_ThrowsException()
    {
        int givenPlayerId = 1;
        int matchPlayerid = 1;
        var badPlayerScore = new PlayerScore
        {
            Kills = -1,
            Deaths = -1,
            Assists = -1
        };

        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player()
        {
            Id = givenPlayerId
        });
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new DataAccess.Entities.Match
            {
                Status = Status.InProgress,
                Competitors = new()
                {
                    new Team
                    {
                        Players = new()
                        {
                            new Player { Id = matchPlayerid }
                        }
                    }
                }
            });

        Assert.ThrowsAsync<BadRequestException>(() =>
            _playerScoreService.CreatePlayerScoreAsync(It.IsAny<string>(), It.IsAny<int>(), badPlayerScore));
    }

    [Test]
    public async Task CreatePlayerScoreAsync_ValidData_Ok()
    {
        int givenPlayerId = 1;
        int matchPlayerid = 1;
        var goodPlayerScore = new PlayerScore
        {
            Kills = 1,
            Deaths = 1,
            Assists = 1
        };

        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new Player()
        {
            Id = givenPlayerId
        });
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new DataAccess.Entities.Match
            {
                Status = Status.InProgress,
                Competitors = new()
                {
                    new Team
                    {
                        Players = new()
                        {
                            new Player { Id = matchPlayerid }
                        }
                    }
                }
            });

        await _playerScoreService.CreatePlayerScoreAsync(
            It.IsAny<string>(), It.IsAny<int>(), goodPlayerScore);

        _userRepositoryMock.Verify(x => x.GetPlayerByUserIdAsync(It.IsAny<string>()), Times.Once);
        _matchRepositoryMock.Verify(x => x.GetMatchByIdAsync(It.IsAny<int>()), Times.Once);
        _playerScoreRepositoryMock.Verify(x =>
            x.AssignPlayerScoreToPlayerAsync(It.IsAny<Player>(), It.IsAny<PlayerScore>()), Times.Once);
    }

    [Test]
    public void GetPlayerKdaAsync_PlayerNotFound_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(It.IsAny<string>())).ReturnsAsync(default(Player));

        Assert.ThrowsAsync<NotFoundException>(() => _playerScoreService.GetPlayerKdaAsync(It.IsAny<string>()));
    }

    public static IEnumerable<TestCaseData> KdaTestCases()
    {
        yield return new TestCaseData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, 2);
        yield return new TestCaseData(new[] { 1, 2, 4 }, new[] { 2, 2, 3 }, new[] { 1, 2, 3 }, (3 + 2 + 7 / 3d) / 3d);
        yield return new TestCaseData(new[] { 1, 2, 4 }, new[] { 2, 2, 3 }, new[] { 0, 0, 0 }, 14 / 3d);
        yield return new TestCaseData(new[] { 0, 0, 0 }, new[] { 0, 0, 0 }, new[] { 0, 0, 0 }, 0);
    }

    [Test]
    [TestCaseSource(nameof(KdaTestCases))]
    public async Task GetPlayerKdaAsync_ValidData_CorrectlyCallculatesKda(int[] kills, int[] assists, int[] deaths, double expectedKda)
    {
        string playerUserId = "id";

        _userRepositoryMock.Setup(x => x.GetPlayerByUserIdAsync(playerUserId)).ReturnsAsync(new Player
        {
            Scores = new()
            {
                new PlayerScore
                {
                    Kills = kills[0],
                    Assists = assists[0],
                    Deaths = deaths[0]
                },
                new PlayerScore
                {
                    Kills = kills[1],
                    Assists = assists[1],
                    Deaths = deaths[1]
                },
                new PlayerScore
                {
                    Kills = kills[2],
                    Assists = assists[2],
                    Deaths = deaths[2]
                },
            }
        });

        var kda = await _playerScoreService.GetPlayerKdaAsync(playerUserId);

        Assert.That(kda, Is.EqualTo(expectedKda));
    }

    [Test]
    public void GetTeamKda_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(() => _playerScoreService.GetTeamKdaAsync(It.IsAny<int>()));
    }

    [Test]
    public void GetTeamKda_TeamHasNotCompletedAnyMatches_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Players = new()
            {
                new Player()
            }
        });

        Assert.ThrowsAsync<BadRequestException>(() => _playerScoreService.GetTeamKdaAsync(It.IsAny<int>()));
    }

    public static IEnumerable<TestCaseData> TeamKdaTestCases()
    {
        yield return new TestCaseData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, 2);
        yield return new TestCaseData(new[] { 1, 2, 4 }, new[] { 2, 2, 3 }, new[] { 1, 2, 3 }, (3 + 2 + 7 / 3d) / 3d);
        yield return new TestCaseData(new[] { 1, 2, 4 }, new[] { 2, 2, 3 }, new[] { 0, 0, 0 }, 14 / 3d);
        yield return new TestCaseData(new[] { 0, 0, 0 }, new[] { 0, 0, 0 }, new[] { 0, 0, 0 }, 0);
    }

    [Test]
    [TestCaseSource(nameof(TeamKdaTestCases))]
    public async Task GetTeamKdaAsync_ValidData_CorrectlyCallculatesKda(int[] kills, int[] assists, int[] deaths, double expectedKda)
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Players = new()
            {
                new Player
                {
                    Scores = new()
                    {
                        new PlayerScore
                        {
                            Kills = kills[0],
                            Assists = assists[0],
                            Deaths = deaths[0]
                        },
                        new PlayerScore
                        {
                            Kills = kills[1],
                            Assists = assists[1],
                            Deaths = deaths[1]
                        },
                        new PlayerScore
                        {
                            Kills = kills[2],
                            Assists = assists[2],
                            Deaths = deaths[2]
                        },
                    }
                },
                new Player
                {
                    Scores = new()
                    {
                        new PlayerScore
                        {
                            Kills = kills[0],
                            Assists = assists[0],
                            Deaths = deaths[0]
                        },
                        new PlayerScore
                        {
                            Kills = kills[1],
                            Assists = assists[1],
                            Deaths = deaths[1]
                        },
                        new PlayerScore
                        {
                            Kills = kills[2],
                            Assists = assists[2],
                            Deaths = deaths[2]
                        },
                    }
                }
            }
        });

        var kda = await _playerScoreService.GetTeamKdaAsync(It.IsAny<int>());

        Assert.That(kda, Is.EqualTo(expectedKda));
    }

    [Test]
    public void GetPlayerScoresByTeamId_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(r => r.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(
            () => _playerScoreService.GetPlayerScoresByTeamId(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
    }

    [Test]
    public async Task GetPlayerScoresByTeamId_WhenTeamHasScores_ShouldReturnDailyPlayerScores()
    {
        int teamId = 1;
        var team = new Team
        {
            Id = teamId,
            Players = new List<Player>
            {
                new Player { Scores = new List<PlayerScore>
                {
                        new PlayerScore { Kills = 3, Assists = 1, Deaths = 2, CreatedAt = DateTime.Today.AddDays(-1) },
                        new PlayerScore { Kills = 2, Assists = 2, Deaths = 1, CreatedAt = DateTime.Today.AddDays(-1) },
                        new PlayerScore { Kills = 1, Assists = 0, Deaths = 2, CreatedAt = DateTime.Today },
                    }
                },
                new Player { Scores = new List<PlayerScore>
                {
                        new PlayerScore { Kills = 1, Assists = 3, Deaths = 1, CreatedAt = DateTime.Today.AddDays(-1) },
                        new PlayerScore { Kills = 2, Assists = 2, Deaths = 2, CreatedAt = DateTime.Today },
                    }
                }
            }
        };
        _teamRepositoryMock.Setup(r => r.GetTeamByIdAsync(teamId)).ReturnsAsync(team);

        var from = DateTime.Today.AddDays(-1);
        var to = DateTime.Today;
        var result = await _playerScoreService.GetPlayerScoresByTeamId(teamId, from, to);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));

        var firstDailyScore = result.First();
        Assert.That(firstDailyScore.Date, Is.EqualTo(DateTime.Today.AddDays(-1)));
        Assert.That(firstDailyScore.TotalKills, Is.EqualTo(6));
        Assert.That(firstDailyScore.TotalAssists, Is.EqualTo(6));
        Assert.That(firstDailyScore.TotalDeaths, Is.EqualTo(4));

        var secondDailyScore = result.Last();
        Assert.That(secondDailyScore.Date, Is.EqualTo(DateTime.Today));
        Assert.That(secondDailyScore.TotalKills, Is.EqualTo(3));
        Assert.That(secondDailyScore.TotalAssists, Is.EqualTo(2));
        Assert.That(secondDailyScore.TotalDeaths, Is.EqualTo(4));
    }
}
