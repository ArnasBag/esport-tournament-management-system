using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface ITeamRepository
{
    Task<Team> CreateTeamAsync(Team team);
}
