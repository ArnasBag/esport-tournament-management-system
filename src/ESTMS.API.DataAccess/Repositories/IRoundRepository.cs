using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface IRoundRepository
{
    public Task<Round?> GetRoundByIdAsync(int roundId);
}