using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserIdProvider _userIdProvider;

    public TournamentService(ITournamentRepository tournamentRepository, IUserRepository userRepository,
        IUserIdProvider userIdProvider)
    {
        _tournamentRepository = tournamentRepository;
        _userRepository = userRepository;
        _userIdProvider = userIdProvider;
    }

    public async Task<List<Tournament>> GetAllTournamentsAsync()
    {
        var tournaments = await _tournamentRepository.GetAllTournamentsAsync();

        return tournaments;
    }

    public async Task<Tournament> GetTournamentByIdAsync(int id)
    {
        var tournament = await _tournamentRepository.GetTournamentByIdAsync(id) ??
                         throw new NotFoundException("Tournament with this id does not exist.");

        return tournament;
    }

    public async Task<Tournament> CreateTournamentAsync(Tournament tournament)
    {
        //var tournamentManager = await _userRepository.GetTournamentManagerByUserIdAsync(_userIdProvider.UserId!);

        //tournament.Manager = tournamentManager!;

        var createdTournament = await _tournamentRepository.CreateTournamentAsync(tournament);

        return createdTournament;
    }

    public async Task<Tournament> UpdateTournamentAsync(int id, Tournament updatedTournament)
    {
        throw new NotImplementedException();
    }

    public async Task<Tournament> UpdateTournamentWinnerAsync(int id, TournamentWinner updatedWinner)
    {
        throw new NotImplementedException();
    }

    public async Task<Tournament> UpdateTournamentStatusAsync(int id, Status updatedStatus)
    {
        throw new NotImplementedException();
    }

    public async Task<Tournament> GetTournamentByTournamentManagerId(string id)
    {
        var tournament = await _tournamentRepository.GetTournamentByTournamentManagerId(id)
                         ?? throw new NotFoundException("Tournament with this id was not found.");

        return tournament;
    }
}