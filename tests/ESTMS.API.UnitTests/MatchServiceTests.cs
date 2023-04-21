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

    private IMatchService _matchService;

    [SetUp]
    public void Setup()
    {
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _playerScoreRepositoryMock = new Mock<IPlayerScoreRepository>();

        _matchService = new MatchService(_matchRepositoryMock.Object, _playerScoreRepositoryMock.Object);
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
    public async Task UpdateMatchStatusAsync_ValidData_Ok()
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
                    }
                }
            }
        });
        _playerScoreRepositoryMock.Setup(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>())).ReturnsAsync(new List<PlayerScore>()
        {
            new PlayerScore()
        });

        await _matchService.UpdateMatchStatusAsync(It.IsAny<int>(), Status.Done);

        _matchRepositoryMock.Verify(x => x.GetMatchByIdAsync(It.IsAny<int>()), Times.Once);
        _playerScoreRepositoryMock.Verify(x => x.GetPlayerScoresByMatchIdAsync(It.IsAny<int>()), Times.Once);
        _matchRepositoryMock.Verify(x => x.UpdateMatchAsync(It.IsAny<DataAccess.Entities.Match>()), Times.Once);

    }
}
