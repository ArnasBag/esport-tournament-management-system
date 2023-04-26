using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserIdProvider _userIdProvider;
    private readonly IRoundRepository _roundRepository;

    public TournamentService
    (
        ITournamentRepository tournamentRepository,
        IUserRepository userRepository,
        IUserIdProvider userIdProvider,
        ITeamRepository teamRepository,
        IRoundRepository roundRepository)
    {
        _tournamentRepository = tournamentRepository;
        _userRepository = userRepository;
        _userIdProvider = userIdProvider;
        _teamRepository = teamRepository;
        _roundRepository = roundRepository;
    }

    public async Task<List<Tournament>> GetAllTournamentsAsync(string? userId = null)
    {
        if (userId is null)
        {
            return await _tournamentRepository.GetAllTournamentsAsync();
        }

        var tournamentManager = await _userRepository.GetTournamentManagerByUserIdAsync(userId)
                                ?? throw new NotFoundException("Tournament manager with this id does not exist.");

        return tournamentManager.Tournaments;
    }

    public async Task<Tournament> GetTournamentByIdAsync(int id)
    {
        var tournament = await _tournamentRepository.GetTournamentByIdAsync(id) ??
                         throw new NotFoundException("Tournament with this id does not exist.");

        return tournament;
    }

    public async Task<Tournament> CreateTournamentAsync(Tournament tournament)
    {
        var tournamentManager = await _userRepository.GetTournamentManagerByUserIdAsync(_userIdProvider.UserId!);

        tournament.Manager = tournamentManager!;
        tournament.Status = Status.NotStarted;

        var createdTournament = await _tournamentRepository.CreateTournamentAsync(tournament);

        return createdTournament;
    }

    public async Task<Tournament> UpdateTournamentAsync(int id, Tournament updatedTournament)
    {
        var tournament = await _tournamentRepository.GetTournamentByIdAsync(id)
                         ?? throw new NotFoundException("Tournament with this id doesn't exist.");

        tournament.Name = updatedTournament.Name;
        tournament.Description = updatedTournament.Description;
        tournament.StartDate = updatedTournament.StartDate;
        tournament.EndDate = updatedTournament.EndDate;

        return await _tournamentRepository.UpdateTournamentAsync(tournament);
    }

    public async Task<Tournament> UpdateTournamentWinnerAsync(int id, int updatedWinner)
    {
        var winnerTeam = await _teamRepository.GetTeamByIdAsync(updatedWinner) ??
                         throw new NotFoundException("Team with this id doesn't exist.");

        var tournament = await _tournamentRepository.GetTournamentByIdAsync(id)
                         ?? throw new NotFoundException("Tournament with this id doesn't exist.");

        if (!tournament.Teams.Any(t => t.Id == winnerTeam.Id))
        {
            throw new NotFoundException("Team with this id did not participate in this tournament");
        }

        tournament.Winner = new TournamentWinner
        {
            TournamentId = tournament.Id,
            Tournament = tournament,
            WinnerTeam = winnerTeam
        };

        return await _tournamentRepository.UpdateTournamentAsync(tournament);
    }

    public async Task<Tournament> UpdateTournamentStatusAsync(int id, Status updatedStatus)
    {
        var tournament = await _tournamentRepository.GetTournamentByIdAsync(id)
                         ?? throw new NotFoundException("Tournament with this id doesn't exist.");

        var rounds = tournament.Rounds ?? throw new BadRequestException("Tournament has no rounds.");

        Status status = tournament.Status;

        switch (updatedStatus)
        {
            case Status.InProgress:
                if (tournament.Teams.Count < 2)
                    throw new BadRequestException("Tournament has too little teams to start.");

                if (rounds.First().Matches.Count < 1)
                    throw new BadRequestException("Tournament has no Matches.");

                status = Status.InProgress;
                break;

            case Status.Done:
                if (tournament.Winner is null)
                    throw new BadRequestException("Cannot finish the tournament. Tournament winner is not set.");

                foreach (var round in rounds)
                {
                    if (round.Matches.Exists(match => match.Status != Status.Done))
                    {
                        throw new BadRequestException("Cannot finish the tournament. Some matches were not played.");
                    }
                }

                status = Status.Done;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(updatedStatus), updatedStatus, null);
        }

        tournament.Status = status;

        return await _tournamentRepository.UpdateTournamentAsync(tournament);
    }

    public async Task<Tournament> GetTournamentByTournamentManagerId(string id)
    {
        var tournament = await _tournamentRepository.GetTournamentByTournamentManagerId(id)
                         ?? throw new NotFoundException("Tournament with this id was not found.");

        return tournament;
    }

    public async Task<Tournament> GenerateBracket(int id)
    {
        var tournament = await _tournamentRepository.GetTournamentByIdAsync(id)
                         ?? throw new NotFoundException("Tournament with this id does not exist.");

        var teamsCount = tournament.Teams.Count;

        if (teamsCount < 2)
            throw new BadRequestException("Cannot create bracket because tournament has less than 2 teams registered.");

        if (tournament.Status is Status.Done or Status.InProgress)
            throw new BadRequestException(
                "Cannot create bracket because tournament is started or has already finished.");

        if (!IsTournamentPerfect(teamsCount))
        {
            throw new BadRequestException("Cannot create bracket because team count is not a perfect power of 2.");
        }

        var competitors = new List<Tuple<int, Team>>();

        foreach (var team in tournament.Teams)
        {
            competitors.Add(new Tuple<int, Team>(await CalculateTeamMmr(team.Id), team));
        }

        var orderedCompetitorsByMmr = competitors.OrderByDescending(x => x.Item1).ToList();

        var firstRound = new Round
        {
            Matches = new List<Match>()
        };

        var matchCount = teamsCount / 2;

        for (var i = 0; i < matchCount; i++)
        {
            var match = new Match
            {
                Status = Status.NotStarted,
                Competitors = new List<Team>()
            };

            match.Competitors.Add(orderedCompetitorsByMmr[i * 2].Item2);
            match.Competitors.Add(orderedCompetitorsByMmr[i * 2 + 1].Item2);

            firstRound.Matches.Add(match);
        }

        tournament.Rounds = new List<Round> { firstRound };
        tournament.Status = Status.InProgress;

        return await _tournamentRepository.UpdateTournamentAsync(tournament);
    }

    private static bool IsTournamentPerfect(int teamCount)
    {
        return (teamCount != 0) && ((teamCount & (teamCount - 1)) == 0);
    }

    private async Task<int> CalculateTeamMmr(int teamId)
    {
        var team = await _teamRepository.GetTeamByIdAsync(teamId)
                   ?? throw new NotFoundException("Team with this id doesn't exist.");

        var sum = 0;

        foreach (var player in team.Players)
        {
            sum += player.Mmr;
        }

        var teamMmr = sum / team.Players.Count;

        return teamMmr;
    }

    public async Task<Tournament> UpdateTournamentBracket(int roundId)
    {
        var round = await _roundRepository.GetRoundByIdAsync(roundId);

        var tournament = await _tournamentRepository.GetTournamentByIdAsync(round.Tournament.Id);

        if (round.Matches.Any(m => m.Status != Status.Done))
        {
            return tournament;
        }

        var winners = round.Matches.OrderBy(m => m.Id).Select(w => w.Winner.WinnerTeamId).ToList();

        if (winners.Count == 1)
        {
            return await UpdateTournamentWinnerAsync(tournament.Id, winners.First());
        }

        var competitors = new List<Team>();

        foreach (var winner in winners)
        {
            competitors.Add(await _teamRepository.GetTeamByIdAsync(winner));
        }

        var nextRound = new Round
        {
            Matches = new List<Match>()
        };

        var matchCount = winners.Count / 2;

        for (var i = 0; i < matchCount; i++)
        {
            var match = new Match
            {
                Status = Status.NotStarted,
                Competitors = new List<Team>()
            };

            match.Competitors.Add(competitors[i * 2]);
            match.Competitors.Add(competitors[i * 2 + 1]);

            nextRound.Matches.Add(match);
        }

        tournament.Rounds.Add(nextRound);

        return await _tournamentRepository.UpdateTournamentAsync(tournament);
    }
}