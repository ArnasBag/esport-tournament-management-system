using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services.Tournaments;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

public class RoundServiceTests
{
    private Mock<IRoundRepository> _roundRepositoryMock;
    private Mock<ITournamentRepository> _tournamentRepositoryMock;

    private IRoundService _roundService;

    [SetUp]
    public void Setup()
    {
        _roundRepositoryMock = new Mock<IRoundRepository>();
        _tournamentRepositoryMock = new Mock<ITournamentRepository>();

        _roundService = new RoundService(_roundRepositoryMock.Object, _tournamentRepositoryMock.Object);
    }

    [Test]
    public void GetRoundByIdAsync_RoundNotFound_ThrowsException()
    {
        _roundRepositoryMock.Setup(x => x.GetRoundByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Round));

        Assert.ThrowsAsync<NotFoundException>(() => _roundService.GetRoundByIdAsync(It.IsAny<int>()));
    }

    [Test]
    public async Task GetRoundByIdAsync_ValidData_Ok()
    {
        _roundRepositoryMock.Setup(x => x.GetRoundByIdAsync(It.IsAny<int>())).ReturnsAsync(new Round());

        await _roundService.GetRoundByIdAsync(It.IsAny<int>());

        _roundRepositoryMock.Verify(x => x.GetRoundByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public void GetTournamentRounds_TournamentNotFound_ThrowsException()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Tournament));

        Assert.ThrowsAsync<NotFoundException>(() => _roundService.GetTournamentRounds(It.IsAny<int>()));
    }

    [Test]
    public async Task GetTournamentRounds_ValidData_Ok()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(new Tournament());

        await _roundService.GetTournamentRounds(It.IsAny<int>());

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(It.IsAny<int>()), Times.Once);
        _roundRepositoryMock.Verify(x => x.GetRoundsByTournamentId(It.IsAny<int>()), Times.Once);
    }
}
