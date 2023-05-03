using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services.Files;
using ESTMS.API.Services.Players;
using Microsoft.AspNetCore.Http;
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
    private Mock<IFormFile> _fileMock;

    [SetUp]
    public void Setup()
    {
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _fileUploaderMock = new Mock<IFileUploader>();
        _fileMock = new Mock<IFormFile>();

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
    public async Task GetAllPlayersAsync_ValidData_Ok()
    {
        await _playerService.GetAllPlayersAsync();

        _playerRepositoryMock.Verify(x => x.GetAllPlayersAsync(), Times.Once);
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

    [Test]
    public void UpdatePlayerAsync_PlayerNotFound_ThrowsException()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<string>())).ReturnsAsync(default(Player));

        Assert.ThrowsAsync<NotFoundException>(
            () => _playerService.UpdatePlayerAsync(It.IsAny<string>(), It.IsAny<Player>(), It.IsAny<IFormFile>()));
    }

    [Test]
    public async Task UpdatePlayerAsync_ProfilePictureIsNull_UpdatesPlayerOnly()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<string>())).ReturnsAsync(new Player
        {
            ApplicationUser = new()
        });

        await _playerService.UpdatePlayerAsync(It.IsAny<string>(), new Player
        {
            ApplicationUser = new()
        }, null);

        _playerRepositoryMock.Verify(x => x.GetPlayerByIdAsync(It.IsAny<string>()), Times.Once);
        _playerRepositoryMock.Verify(x => x.UpdatePlayerAsync(It.IsAny<Player>()), Times.Once);
        _fileUploaderMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
        _fileUploaderMock.Verify(x => x.UploadFileAsync(It.IsAny<IFormFile>()), Times.Never);
    }

    [Test]
    public async Task UpdatePlayerAsync_ProfilePictureIsNotNull_UpdatesPlayer()
    {
        _playerRepositoryMock.Setup(x => x.GetPlayerByIdAsync(It.IsAny<string>())).ReturnsAsync(new Player
        {
            ApplicationUser = new()
        });

        await _playerService.UpdatePlayerAsync(It.IsAny<string>(), new Player
        {
            ApplicationUser = new()
        }, _fileMock.Object);

        _playerRepositoryMock.Verify(x => x.GetPlayerByIdAsync(It.IsAny<string>()), Times.Once);
        _playerRepositoryMock.Verify(x => x.UpdatePlayerAsync(It.IsAny<Player>()), Times.Once);
        _fileUploaderMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Once);
        _fileUploaderMock.Verify(x => x.UploadFileAsync(It.IsAny<IFormFile>()), Times.Once);
    }
}