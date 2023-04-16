using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface ITeamService
{
    Task<Team> CreateTeamAsync(Team team);
    Task DeactivateTeamAsync(int id);
    Task<Team> UpdateTeamAsync(int id, Team updatedTeam);
}
