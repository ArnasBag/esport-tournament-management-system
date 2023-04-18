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
}