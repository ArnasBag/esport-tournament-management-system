using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;

    public TeamService(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public async Task<Team> CreateTeamAsync(Team team)
    {
        var createdTeam = await _teamRepository.CreateTeamAsync(team);

        return createdTeam;
    }
}
