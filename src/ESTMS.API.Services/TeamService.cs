using ESTMS.API.Core.Exceptions;
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

    public async Task<Team> UpdateTeamAsync(int id, Team updatedTeam)
    {
        var team = await _teamRepository.GetTeamByIdAsync(id) 
            ?? throw new NotFoundException("Team with this id doesn't exist.");

        team.Name = updatedTeam.Name;
        team.Description = updatedTeam.Description;

        return await _teamRepository.UpdateTeamAsync(team);
    }

    public async Task DeactivateTeamAsync(int id)
    {
        var team = await _teamRepository.GetTeamByIdAsync(id)
            ?? throw new NotFoundException("Team with this id doesn't exist.");

        team.Deleted = true;

        await _teamRepository.UpdateTeamAsync(team);
    }
}
