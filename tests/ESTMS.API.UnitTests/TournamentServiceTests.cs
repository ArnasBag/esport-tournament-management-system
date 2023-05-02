using AutoFixture;
using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services.Auth;
using ESTMS.API.Services.Tournaments;
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
    private Mock<IRoundRepository> _roundRepositoryMock;
    private Mock<ITournamentService> _tournamentServiceMock;

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
        _roundRepositoryMock = new Mock<IRoundRepository>();

        _tournamentService = new TournamentService(_tournamentRepositoryMock.Object,
            _userRepositoryMock.Object, _userIdProviderMock.Object, _teamRepositoryMock.Object,
            _roundRepositoryMock.Object);

        _tournamentServiceMock = new Mock<ITournamentService>();
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
            },
            MaxTeamCount = 2
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        _tournamentService.UpdateTournamentStatusAsync(tournamentId, status);

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(tournamentId), Times.Once);
        _tournamentRepositoryMock.Verify(x => x.UpdateTournamentAsync(tournament), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatusAsync_ToStatusInProgress_AlreadyInProgress_ThrowsException()
    {
        var tournament = new Tournament
        {
            Rounds = Fixture.CreateMany<Round>(2).ToList(),
            Id = 1,
            Status = Status.InProgress,
            Teams = new List<Team>()
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.InProgress));

        Assert.That(ex.Message, Is.EqualTo("Tournament is already in progress."));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatusAsync_ToStatusInProgress_NotEnoughTeams_ThrowsException()
    {
        var tournament = new Tournament
        {
            Rounds = Fixture.CreateMany<Round>(2).ToList(),
            Id = 1,
            Status = Status.NotStarted,
            Teams = Fixture.CreateMany<Team>(1).ToList(),
            MaxTeamCount = 2
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.InProgress));

        Assert.That(ex.Message, Is.EqualTo("Tournament has too little teams to start."));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatusAsync_ToStatusInProgress_NoMatches_ThrowsException()
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
            Rounds = new List<Round>
            {
                new()
                {
                    Matches = new List<Match>()
                }
            },
            MaxTeamCount = 2
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.InProgress));

        Assert.That(ex.Message, Is.EqualTo("Tournament has no Matches."));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatusAsync_ToStatusDone_WinnerNotSet_ThrowsException()
    {
        var tournament = new Tournament
        {
            Rounds = Fixture.CreateMany<Round>(2).ToList(),
            Id = 1,
            Status = Status.InProgress,
            Teams = new List<Team>()
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.Done));

        Assert.That(ex.Message, Is.EqualTo("Cannot finish the tournament. Tournament winner is not set."));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatusAsync_ToStatusDone_AlreadyDone_ThrowsException()
    {
        var tournament = new Tournament
        {
            Rounds = Fixture.CreateMany<Round>(2).ToList(),
            Id = 1,
            Status = Status.Done,
            Teams = new List<Team>()
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.Done));

        Assert.That(ex.Message, Is.EqualTo("Tournament is already finished."));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public void UpdateTournamentStatusAsync_ToStatusDone_MatchesInProgress_ThrowsException()
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
            Winner = new TournamentWinner(),
            Status = Status.NotStarted,
            Rounds = new List<Round> { new Round { Matches = matches } },
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.UpdateTournamentStatusAsync(1, Status.Done));

        Assert.That(ex.Message, Is.EqualTo("Cannot finish the tournament. Some matches were not played."));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(1), Times.Once);
    }

    [Test]
    public async Task CreateBrackets_ValidData_Ok()
    {
        var teamCount = 8;
        var sequenceNumber = 1;
        Fixture.Customize<Team>(c => c.With(team => team.Id, () => sequenceNumber++));

        var teams = Fixture.Build<Team>().With(x => x.Players, Fixture.CreateMany<Player>(3).ToList())
            .CreateMany(teamCount).ToList();

        var tournament = new Tournament
        {
            MaxTeamCount = teamCount,
            Teams = teams
        };

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>())).ReturnsAsync(tournament);

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(teams[0]);

        _tournamentRepositoryMock.SetupSequence(x => x.UpdateTournamentAsync(It.IsAny<Tournament>()))
            .ReturnsAsync(new Tournament
            {
                MaxTeamCount = teamCount,
                Teams = teams,
            })
            .ReturnsAsync(new Tournament
            {
                MaxTeamCount = teamCount,
                Teams = teams,
                Status = Status.InProgress,
                Rounds = new List<Round>
                {
                    new Round()
                    {
                        Matches = new List<Match>(new Match[4])
                    }
                }
            });

        //_tournamentServiceMock.Setup(x => x.UpdateTournamentStatusAsync(It.IsAny<int>(), It.IsAny<Status>()))
        //    .ReturnsAsync(new Tournament());

        var actualTournament = await _tournamentService.CreateBracket(It.IsAny<int>());

        Assert.That(actualTournament.Rounds[0].Matches.Count, Is.EqualTo(teamCount / 2));
        Assert.That(actualTournament.Status, Is.EqualTo(Status.InProgress));

        _tournamentRepositoryMock.Verify(x => x.GetTournamentByIdAsync(It.IsAny<int>()), Times.Exactly(2));
        _tournamentRepositoryMock.Verify(x => x.UpdateTournamentAsync(It.IsAny<Tournament>()), Times.Exactly(2));
    }

    [Test]
    public async Task CreateBrackets_TournamentNotFound_ThrowsException()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(default(Tournament));

        var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
            await _tournamentService.CreateBracket(It.IsAny<int>()));

        Assert.That(ex.Message, Is.EqualTo("Tournament with this id does not exist."));
    }

    [Test]
    public void CreateBrackets_TournamentIsInProgress_ThrowsException()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Tournament
            {
                Status = Status.InProgress,
                Teams = Fixture.CreateMany<Team>(7).ToList()
            });

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.CreateBracket(It.IsAny<int>()));

        Assert.That(ex.Message,
            Is.EqualTo("Cannot create bracket because tournament is started or has already finished."));
    }

    [Test]
    public void CreateBracket_LowTeamCount_ThrowsException()
    {
        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Tournament
            {
                Status = Status.NotStarted,
                MaxTeamCount = 8,
                Teams = Fixture.CreateMany<Team>(7).ToList()
            });

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.CreateBracket(It.IsAny<int>()));

        Assert.That(ex.Message,
            Is.EqualTo("Cannot create bracket because tournament has not enough teams registered."));
    }

    [Test]
    public async Task UpdateBracket_MatchesNotDone_Ok()
    {
        var round = new Round
        {
            Matches = new List<Match>
            {
                new Match
                {
                    Status = Status.InProgress,
                }
            },
            Tournament = new Tournament
            {
                Id = 1
            }
        };

        _roundRepositoryMock.Setup(x => x.GetRoundByIdAsync(It.IsAny<int>())).ReturnsAsync(round);

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Tournament());


        await _tournamentService.UpdateTournamentBracket(It.IsAny<int>());

        _tournamentRepositoryMock.Verify(x => x.UpdateTournamentAsync(It.IsAny<Tournament>()), Times.Never);
    }

    [Test]
    public async Task UpdateBracket_PlayedLastRound_SetsTournamentWinner()
    {
        var round = new Round
        {
            Matches = new List<Match>
            {
                new Match
                {
                    Status = Status.Done,
                    Winner = new MatchWinner
                    {
                        WinnerTeamId = 1
                    }
                }
            },
            Tournament = new Tournament
            {
                Id = 1
            }
        };

        var team = new Team
        {
            Id = 1
        };

        _roundRepositoryMock.Setup(x => x.GetRoundByIdAsync(It.IsAny<int>())).ReturnsAsync(round);

        _tournamentRepositoryMock.SetupSequence(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Tournament())
            .ReturnsAsync(new Tournament
            {
                Teams = new List<Team> { team }
            })
            .ReturnsAsync(new Tournament
            {
                Status = Status.InProgress,
                Winner = new TournamentWinner(),
                Rounds = new List<Round> { round }
            });

        _teamRepositoryMock.Setup(x => x.GetTeamByIdAsync(It.IsAny<int>())).ReturnsAsync(team);


        _tournamentRepositoryMock.Setup(x => x.UpdateTournamentAsync(It.IsAny<Tournament>()))
            .ReturnsAsync(new Tournament());

        await _tournamentService.UpdateTournamentBracket(It.IsAny<int>());

        _tournamentRepositoryMock.Verify(x => x.UpdateTournamentAsync(It.IsAny<Tournament>()), Times.Exactly(2));
    }

    [Test]
    public async Task UpdateBracket_ValidData_UpdatesNextRound()
    {
        var round = new Round
        {
            Matches = Fixture.Build<Match>().With(x => x.Status, Status.Done).CreateMany(4).ToList(),
            Tournament = new Tournament
            {
                Id = 1
            }
        };

        _roundRepositoryMock.Setup(x => x.GetRoundByIdAsync(It.IsAny<int>())).ReturnsAsync(round);

        _tournamentRepositoryMock.Setup(x => x.GetTournamentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Tournament
            {
                Rounds = new List<Round>()
            });


        await _tournamentService.UpdateTournamentBracket(It.IsAny<int>());

        _tournamentRepositoryMock.Verify(x => x.UpdateTournamentAsync(It.IsAny<Tournament>()), Times.Once);
    }

    [Test]
    public async Task CreateTournamentAsync_MaxTeamCountNotPowerOf2_ThrowsException()
    {
        var tournament = new Tournament
        {
            MaxTeamCount = 3
        };

        var ex = Assert.ThrowsAsync<BadRequestException>(async () =>
            await _tournamentService.CreateTournamentAsync(tournament));

        Assert.That(ex.Message, Is.EqualTo("Cannot tournament because max team count is not a perfect power of 2."));
    }

    [Test]
    public async Task CreateTournamentAsync_ValidData_Ok()
    {
        var tournament = new Tournament
        {
            Description = "test",
            Name = "test",
            MaxTeamCount = 128
        };

        _userRepositoryMock.Setup(x => x.GetTournamentManagerByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new TournamentManager());

        await _tournamentService.CreateTournamentAsync(tournament);

        _tournamentRepositoryMock.Verify(x => x.CreateTournamentAsync(It.IsAny<Tournament>()), Times.Once);
    }
}