using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class RoundService : IRoundService
{
    private readonly IRoundRepository _roundRepository;

    public RoundService(IRoundRepository roundRepository)
    {
        _roundRepository = roundRepository;
    }

    public async Task<Round> GetRoundByIdAsync(int id)
    {
        return await _roundRepository.GetRoundByIdAsync(id) ??
               throw new NotFoundException("Round with this id doesn't exist.");
    }
}