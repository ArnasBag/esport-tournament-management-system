using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

[TestFixture]
public class PlayerServiceTests
{
    private IPlayerService _playerService;

    private Mock<IPlayerRepository> _playerRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _playerService = new PlayerService(_playerRepositoryMock.Object);
    }

    [Test]
    public async Task GetPlayerByIdAsync_GetsPlayer_Ok()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<int>())).ReturnsAsync(new Player());

        await _playerService.GetPlayerByIdAsync(It.IsAny<int>());

        _playerRepositoryMock.Verify(x => x.GetPlayerByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public void GetPlayerByIdAsync_GetsNullPlayer_ThrowsError()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<int>())).ReturnsAsync((Player)null);

        var result = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _playerService.GetPlayerByIdAsync(It.IsAny<int>()));

        Assert.That(result.Message, Is.EqualTo("Player with this id doesn't exist."));
    }

    [Test]
    public async Task UpdatePlayersRankAsync_GetsPlayer_Ok()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<int>())).ReturnsAsync(new Player());

        await _playerService.UpdatePlayersRankAsync(It.IsAny<int>());

        _playerRepositoryMock.Verify(x => x.GetPlayerByIdAsync(It.IsAny<int>()), Times.Once);
        _playerRepositoryMock.Verify(x => x.UpdatePlayerAsync(It.IsAny<Player>()), Times.Once);
    }

    [Test]
    public void UpdatePlayersRankAsync_GetsNullPlayer_ThrowsError()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<int>())).ReturnsAsync((Player)null);

        var result = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _playerService.UpdatePlayersRankAsync(It.IsAny<int>()));

        Assert.That(result.Message, Is.EqualTo("Player with this id doesn't exist."));
    }

    private static IEnumerable<TestCaseData> UpdatePlayerPointsTestCases()
    {
        yield return new TestCaseData(0, 1, 10, 10);
        yield return new TestCaseData(10, 1, -10, 0);
        yield return new TestCaseData(null, 1, 10, 10);
        yield return new TestCaseData(null, 1, -10, 0);
    }

    [Test]
    [TestCaseSource(nameof(UpdatePlayerPointsTestCases))]
    public async Task UpdatePlayersPointAsync_GetsPlayer_Ok(int? playerPoints, int playerId, int points, int result)
    {
        var player = new Player
        {
            Id = playerId,
            Points = playerPoints
        };

        var expectedPlayer = new Player
        {
            Id = playerId,
            Points = result
        };

        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(playerId)).ReturnsAsync(player);

        _playerRepositoryMock.SetupSequence(x => x.UpdatePlayerAsync(It.IsAny<Player>()))
            .ReturnsAsync(expectedPlayer)
            .ReturnsAsync(expectedPlayer);

        var actualPlayer = await _playerService.UpdatePlayersPointAsync(playerId, points);


        _playerRepositoryMock.Verify(x => x.GetPlayerByIdAsync(playerId), Times.Exactly(2));
        _playerRepositoryMock.Verify(x => x.UpdatePlayerAsync(player), Times.Exactly(2));

        Assert.That(actualPlayer.Points, Is.EqualTo(expectedPlayer.Points));
    }
}