using AutoFixture;
using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using Moq;
using NUnit.Framework;
using Match = ESTMS.API.DataAccess.Entities.Match;

namespace ESTMS.API.UnitTests;

[TestFixture]
public class TournamentServiceTests
{

    private static Fixture Fixture;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<ITournamentRepository> _tournamentRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserIdProvider> _userIdProviderMock;

    private ITournamentService _tournamentService;

    static TournamentServiceTests()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [SetUp]
    public void Setup()
    {
        _tournamentRepositoryMock = new Mock<ITournamentRepository>();
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userIdProviderMock = new Mock<IUserIdProvider>();

        _tournamentService = new TournamentService(_tournamentRepositoryMock.Object,
            _userRepositoryMock.Object, _userIdProviderMock.Object, _teamRepositoryMock.Object);
    }

    [Test]
    public async Task GetAllTournamentsAsync_TournamentManagerUserIdNull_GetsAllTournaments()
    {
        string? nullUserId = null;

        await _tournamentService.GetAllTournamentsAsync(nullUserId);

        _tournamentRepositoryMock.Verify(x => x.GetAllTournamentsAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllTournamentsAsync_TournamentManagerUserIdNotNull_GetsAllTournamentManagerTournaments()
    {
        string nonNullUserId = "id";

        _userRepositoryMock.Setup(x => x.GetTournamentManagerByUserIdAsync(nonNullUserId))
            .ReturnsAsync(new TournamentManager());

        await _tournamentService.GetAllTournamentsAsync(nonNullUserId);

        _userRepositoryMock.Verify(x => x.GetTournamentManagerByUserIdAsync(nonNullUserId), Times.Once);
    }

    [Test]
    public void GetAllTournamentsAsync_NonExistantTournamentManagerId_ThrowsException()
    {
        string nonNullUserId = "id";

        _userRepositoryMock.Setup(x => x.GetTournamentManagerByUserIdAsync(nonNullUserId))
            .ReturnsAsync(default(TournamentManager));

        Assert.ThrowsAsync<NotFoundException>(() => _tournamentService.GetAllTournamentsAsync(nonNullUserId));
    }

    [Test]
    public void GetTournamentByIdAsync_TournamentNotFound_ThrowsException()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Tournament));

        Assert.ThrowsAsync<NotFoundException>(() => _tournamentService.GetTournamentByIdAsync(It.IsAny<int>()));
    }

    [Test]
    public async Task GetTournamentByIdAsync_ValidData_Ok()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(new Tournament());

        await _tournamentService.GetTournamentByIdAsync(It.IsAny<int>());

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task UpdateTournamentAsync_ValidData_Ok()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(new Tournament());

        await _tournamentService.UpdateTournamentAsync(It.IsAny<int>(), new Tournament());

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(It.IsAny<int>()), Times.Once);
        _tournamentRepositoryMock.Verify(x => x.UpdateTournamentAsync(It.IsAny<Tournament>()), Times.Once);
    }

    [Test]
    public void UpdateTournamentAsync_TournamentNotFound_ThrowsException()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Tournament));

        Assert.ThrowsAsync<NotFoundException>(() =>
            _tournamentService.UpdateTournamentAsync(It.IsAny<int>(), new Tournament()));
    }

    [Test]
    public void UpdateTournamentWinnerAsync_ValidData_Ok()
    {
        var team = new Team
        {
            Deleted = false,
            Description = "",
            Name = "",
            Id = 1,
            Matches = new List<Match>(),
            Players = new List<Player>(),
            Tournaments = new List<Tournament>(),
            TeamManager = new TeamManager()
        };

        var tournament = new Tournament
        {
            Id = 1,
            Teams = new List<Team> { team }
        };

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(team);

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        _tournamentService.UpdateTournamentWinnerAsync(tournament.Id, team.Id);

        _tournamentRepositoryMock.Verify(x => x.UpdateTournamentAsync(tournament), Times.Once);
    }

    [Test]
    public void UpdateTournamentWinnerAsync_TeamNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        Assert.ThrowsAsync<NotFoundException>(() =>
            _tournamentService.UpdateTournamentWinnerAsync(It.IsAny<int>(), It.IsAny<int>()));
    }

    [Test]
    public void UpdateTournamentWinnerAsync_TournamentNotFound_ThrowsException()
    {
        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(default(Team));

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Tournament));

        Assert.ThrowsAsync<NotFoundException>(() =>
            _tournamentService.UpdateTournamentWinnerAsync(It.IsAny<int>(), It.IsAny<int>()));
    }

    [Test]
    public void UpdateTournamentWinnerAsync_TeamDidNotParticipateInTournament_ThrowsException()
    {
        var team = new Team
        {
            Deleted = false,
            Description = "",
            Name = "",
            Id = 1,
            Matches = new List<Match>(),
            Players = new List<Player>(),
            Tournaments = new List<Tournament>(),
            TeamManager = new TeamManager()
        };

        var tournament = new Tournament
        {
            Id = 1,
            Teams = new List<Team>()
        };

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(team);

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        Assert.ThrowsAsync<NotFoundException>(() =>
            _tournamentService.UpdateTournamentWinnerAsync(tournament.Id, team.Id));

        _teamRepositoryMock.Verify(x => x.GetTeamByIdAsync(team.Id), Times.Once);
        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(tournament.Id), Times.Once);
    }

    private static IEnumerable<TestCaseData> UpdateTournamentStatusTestCases()
    {
        yield return new TestCaseData(1, Status.InProgress);
        yield return new TestCaseData(1, Status.Done);
    }

    [Test]
    [TestCaseSource(nameof(UpdateTournamentStatusTestCases))]
    public void UpdateTournamentStatusAsync_Ok(int tournamentId, Status status)
    {
        var matches = new List<Match>();

        var match = new Match
        {
            Id = 1,
            Status = Status.Done
        };

        matches.Add(match);
        matches.Add(match);

        var teams = new List<Team>();

        var team = new Team
        {
            Id = 1,
        };

        var team2 = new Team
        {
            Id = 2,
        };

        teams.Add(team);
        teams.Add(team2);

        var tournament = new Tournament
        {
            Id = tournamentId,
            Status = Status.NotStarted,
            Rounds = new List<Round>
            {
                new Round
                {
                    Matches = matches
                }
            },
            Teams = teams,
            Winner = new TournamentWinner
            {
                Id = team.Id,
                Tournament = new Tournament(),
                TournamentId = tournamentId,
                WinnerTeam = team
            }
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        _tournamentService.UpdateTournamentStatusAsync(tournamentId, status);

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(tournamentId), Times.Once);
        _tournamentRepositoryMock.Verify(x => x.UpdateTournamentAsync(tournament), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatus_ToStatusInProgress_LowTeamCount_ThrowsException()
    {
        var tournament = new Tournament
        {
            Id = 1,
            Status = Status.NotStarted,
            Teams = new List<Team>()
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.InProgress));

        Assert.That(ex.Message, Is.EqualTo("Tournament has too little teams to start."));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatusAsync_ToStatusInProgress__NoMatches_ThrowsException()
    {
        var teams = new List<Team>();

        var team = new Team
        {
            Id = 1,
        };

        var team2 = new Team
        {
            Id = 2,
        };

        teams.Add(team);
        teams.Add(team2);

        var tournament = new Tournament
        {
            Id = 1,
            Status = Status.NotStarted,
            Teams = teams,
            Rounds = new List<Round>()
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.InProgress));

        Assert.That(ex.Message, Is.EqualTo("Tournament has no Matches."));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatus_ToStatusDone_MatchesInProgress_ThrowsException()
    {
        var matches = new List<Match>();

        var match = new Match
        {
            Id = 1,
            Status = Status.InProgress
        };

        matches.Add(match);
        matches.Add(match);

        var tournament = new Tournament
        {
            Id = 1,
            Status = Status.NotStarted,
            Rounds = new List<Round> { new Round { Matches = matches } },
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.Done));

        Assert.That(ex.Message, Is.EqualTo("There are still matches in progress"));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public void GenerateBrackets_ValidData_Ok()
    {
        var sequenceNumber = 1;
        Fixture.Customize<Team>(c => c.With(team => team.Id, () => sequenceNumber++));

        var teams = Fixture.CreateMany<Team>(32).ToList();

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(new Tournament
        {
            Teams = teams
        });

        _tournamentService.GenerateBracket(It.IsAny<int>());
    }
}