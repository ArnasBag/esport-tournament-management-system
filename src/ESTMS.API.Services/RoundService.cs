using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class RoundService : IRoundService
{
    private readonly IRoundRepository _roundRepository;
    private readonly ITournamentRepository _tournamentRepository;

    public RoundService(IRoundRepository roundRepository, 
        ITournamentRepository tournamentRepository)
    {
        _roundRepository = roundRepository;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<Round> GetRoundByIdAsync(int id)
    {
        return await _roundRepository.GetRoundByIdAsync(id) ??
               throw new NotFoundException("Round with this id doesn't exist.");
    }

    public async Task<List<Round>> GetTournamentRounds(int tournamentId)
    {
        var tournament = await _tournamentRepository.GetTournamentByIdAsync(tournamentId)
            ?? throw new NotFoundException("Tournament with this id was not found.");

        return await _roundRepository.GetRoundsByTournamentId(tournamentId);
    }
}