using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface ITeamManagerRepository
{
    Task RemoveAsync(TeamManager teamManager);
    Task CreateAsync(TeamManager teamManager);
}
