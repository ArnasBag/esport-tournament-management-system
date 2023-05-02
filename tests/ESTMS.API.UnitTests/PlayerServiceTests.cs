using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services.Files;
using ESTMS.API.Services.Players;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

[TestFixture]
public class PlayerServiceTests
{
    private IPlayerService _playerService;

    private Mock<IPlayerRepository> _playerRepositoryMock;
    private Mock<IMatchRepository> _matchRepositoryMock;
    private Mock<IFileUploader> _fileUploaderMock;

    [SetUp]
    public void Setup()
    {
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _playerService = new PlayerService(_playerRepositoryMock.Object, _fileUploaderMock.Object, _matchRepositoryMock.Object);
    }

    [Test]
    public async Task GetPlayerByIdAsync_GetsPlayer_Ok()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<string>())).ReturnsAsync(new Player());

        await _playerService.GetPlayerByIdAsync(It.IsAny<string>());

        _playerRepositoryMock.Verify(x => x.GetPlayerByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetPlayerByIdAsync_GetsNullPlayer_ThrowsError()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<string>())).ReturnsAsync((Player)null);

        var result = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _playerService.GetPlayerByIdAsync(It.IsAny<string>()));

        Assert.That(result.Message, Is.EqualTo("Player with this id doesn't exist."));
    }

    [Test]
    public async Task UpdatePlayersRankAsync_GetsPlayer_Ok()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<string>())).ReturnsAsync(new Player());

        await _playerService.UpdatePlayersRankAsync(It.IsAny<string>());

        _playerRepositoryMock.Verify(x => x.GetPlayerByIdAsync(It.IsAny<string>()), Times.Once);
        _playerRepositoryMock.Verify(x => x.UpdatePlayerAsync(It.IsAny<Player>()), Times.Once);
    }

    [Test]
    public void UpdatePlayersRankAsync_GetsNullPlayer_ThrowsError()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<string>())).ReturnsAsync((Player)null);

        var result = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _playerService.UpdatePlayersRankAsync(It.IsAny<string>()));

        Assert.That(result.Message, Is.EqualTo("Player with this id doesn't exist."));
    }

    private static IEnumerable<TestCaseData> UpdatePlayerPointsTestCases()
    {
        yield return new TestCaseData(0, Guid.NewGuid().ToString(), 10, 10);
        yield return new TestCaseData(10, Guid.NewGuid().ToString(), -10, 0);
        yield return new TestCaseData(null, Guid.NewGuid().ToString(), 10, 10);
        yield return new TestCaseData(null, Guid.NewGuid().ToString(), -10, 0);
    }

    [Test]
    [TestCaseSource(nameof(UpdatePlayerPointsTestCases))]
    public async Task UpdatePlayersPointAsync_GetsPlayer_Ok(int? playerPoints, string playerId, int points, int result)
    {
        var player = new Player
        {
            ApplicationUser = new ApplicationUser
            {
                Id = playerId
            },
            Points = playerPoints
        };

        var expectedPlayer = new Player
        {
            ApplicationUser = new ApplicationUser
            {
                Id = playerId
            },
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