using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserIdProvider _userIdProvider;

    public TeamService(ITeamRepository teamRepository,
        IUserRepository userRepository,
        IUserIdProvider userIdProvider)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _userIdProvider = userIdProvider;
    }

    public async Task<Team> CreateTeamAsync(Team team)
    {
        var manager = await _userRepository.GetTeamManagerByUserIdAsync(_userIdProvider.UserId!);
     
        team.TeamManager = manager!;

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

    public async Task<Team> GetTeamByIdAsync(int id)
    {
        return await _teamRepository.GetTeamByIdAsync(id) 
            ?? throw new NotFoundException("Team with this id was not found.");    
    }

    public async Task<List<Team>> GetAllTeamsAsync(string? userId = null)
    {
        if(userId == null)
        {
            return await _teamRepository.GetAllTeamsAsync();
        }

        var teamManager = await _userRepository.GetTeamManagerByUserIdAsync(userId)
            ?? throw new NotFoundException("Team manager with this id does not exist.");

        return teamManager.Teams;
    }

    public async Task<Team> GetTeamByTeamManagerId(string teamManagerUserId)
    {
        var team = await _teamRepository.GetTeamByTeamManagerUserId(teamManagerUserId)
            ?? throw new NotFoundException("Team with this id was not found.");

        return team;
    }
}
