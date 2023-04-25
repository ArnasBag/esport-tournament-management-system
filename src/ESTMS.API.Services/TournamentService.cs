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

    public TournamentService(ITournamentRepository tournamentRepository, IUserRepository userRepository,
        IUserIdProvider userIdProvider, ITeamRepository teamRepository)
    {
        _tournamentRepository = tournamentRepository;
        _userRepository = userRepository;
        _userIdProvider = userIdProvider;
        _teamRepository = teamRepository;
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

        Status status = tournament.Status;

        switch (updatedStatus)
        {
            case Status.InProgress:
                if (tournament.Teams.Count < 2)
                    throw new BadRequestException("Tournament has too little teams to start.");

                if (tournament.Matches.Count < 1)
                    throw new BadRequestException("Tournament has no Matches.");

                status = Status.InProgress;
                break;

            case Status.Done:
                if (tournament.Matches.Any(m => m.Status != Status.Done))
                    throw new BadRequestException("There are still matches in progress");

                if (tournament.Winner is null)
                    throw new BadRequestException("Tournament winner is not set.");

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

        if (tournament.Teams.Count < 2)
            throw new BadRequestException("Cannot create bracket because tournament has less than 2 teams registered.");

        //cannot create cuz its in progress || done
        if (tournament.Status is Status.Done or Status.InProgress)
            throw new BadRequestException("Cannot create bracket because tournament is started or has already finished.");


        //cannot create cuz already has matches
        if (tournament.Matches.Count > 0)
            throw new BadRequestException("Cannot create bracket because tournament has already created matches.");


        //List<Match> matches = Matchmake(tournament);

        //matches - sorted teams by MMR rating.
        //sort by date availability.

        return tournament;
    }
}