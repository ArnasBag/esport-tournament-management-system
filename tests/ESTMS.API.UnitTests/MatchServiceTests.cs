﻿using AutoFixture;
using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using Moq;
using NUnit.Framework;
using Match = ESTMS.API.DataAccess.Entities.Match;

namespace ESTMS.API.UnitTests;

public class MatchServiceTests
{
    private static Fixture Fixture;

    private Mock<IMatchRepository> _matchRepositoryMock;
    private Mock<IPlayerScoreRepository> _playerScoreRepositoryMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<IMmrCalculator> _mmrCalculatorMock;
    private Mock<IPlayerRepository> _playerRepositoryMock;
    private Mock<ITournamentService> _tournamentServiceMock;

    private IMatchService _matchService;

    static MatchServiceTests()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [SetUp]
    public void Setup()
    {
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _playerScoreRepositoryMock = new Mock<IPlayerScoreRepository>();
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _mmrCalculatorMock = new Mock<IMmrCalculator>();
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _tournamentServiceMock = new Mock<ITournamentService>();

        _matchService = new MatchService(_matchRepositoryMock.Object, _playerScoreRepositoryMock.Object,
            _teamRepositoryMock.Object, _mmrCalculatorMock.Object, _playerRepositoryMock.Object,
            _tournamentServiceMock.Object);
    }

    [Test]
    public void UpdateMatchStatusAsync_MatchNotFound_ThrowsException()
    {
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(DataAccess.Entities.Match));

        Assert.ThrowsAsync<NotFoundException>(
            () => _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), It.IsAny<Status>()));
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
        _playerScoreRepositoryMock.Setup(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>())).ReturnsAsync(
            new List<PlayerScore>()
            {
                new PlayerScore()
            });

        Assert.ThrowsAsync<BadRequestException>(
            () => _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), Status.Done));
    }

    [Test]
    public void UpdateMatchStatusAsync_MatchStatusDoneAndWinnerNotAssigned_ThrowsException()
    {
        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(new DataAccess.Entities.Match
        {
            Competitors = new List<Team>()
            {
                new Team
                {
                    Players = new List<Player>()
                    {
                        new Player()
                    }
                }
            }
        });
        _playerScoreRepositoryMock.Setup(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>())).ReturnsAsync(
            new List<PlayerScore>()
            {
                new PlayerScore()
            });

        Assert.ThrowsAsync<BadRequestException>(
            () => _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), Status.Done));
    }

    [Test]
    public async Task UpdateMatchStatusAsync_ValidData_Ok()
    {
        var team1 = new Team
        {
            Id = 1,
            Players = new List<Player>
            {
                new Player
                {
                    Mmr = 10,
                    Scores = new List<PlayerScore>()
                    {
                        new PlayerScore
                        {
                            Match = new Match
                            {
                                Id = 1
                            }
                        }
                    }
                }
            }
        };

        var team2 = new Team
        {
            Id = 2,
            Players = new List<Player>
            {
                new Player
                {
                    Mmr = 10,
                    Scores = new List<PlayerScore>()
                    {
                        new PlayerScore
                        {
                            Match = new Match
                            {
                                Id = 1
                            }
                        }
                    }
                }
            }
        };

        var match = new Match
        {
            Id = 1,
            Round = new Round
            {
                Id = 1
            },
            Competitors = new List<Team>
            {
                team1,
                team2
            },
            Winner = new MatchWinner
            {
                WinnerTeamId = 1
            }
        };

        var score = new PlayerScore()
        {
            Match = new Match()
            {
                Id = 1
            }
        };

        _matchRepositoryMock.Setup(x => x.GetMatchByIdAsync(It.IsAny<int>())).ReturnsAsync(match);

        _playerScoreRepositoryMock.Setup(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>())).ReturnsAsync(
            new List<PlayerScore>()
            {
                new PlayerScore(),
                new PlayerScore()
            });

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team
        {
            Id = 1,
            Players = new List<Player>
            {
                new Player
                {
                    Mmr = 10,
                    Scores = new List<PlayerScore>
                    {
                        score
                    }
                }
            }
        });

        await _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), Status.Done);

        _matchRepositoryMock.Verify(x => x.GetMatchByIdAsync(It.IsAny<int>()), Times.Once);
        _playerScoreRepositoryMock.Verify(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>()), Times.Once);
        _matchRepositoryMock.Verify(x => x.UpdateMatchAsync(It.IsAny<DataAccess.Entities.Match>()), Times.Once);
    }
}