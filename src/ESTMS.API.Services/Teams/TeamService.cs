using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services.Auth;
using ESTMS.API.Services.Files;
using ESTMS.API.Services.Matches;
using Microsoft.AspNetCore.Http;

namespace ESTMS.API.Services.Teams;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserIdProvider _userIdProvider;
    private readonly IFileUploader _fileUploader;
    private readonly IMmrCalculator _mmrCalculator;
    private readonly IPlayerRepository _playerRepository;

    public TeamService(ITeamRepository teamRepository,
        IUserRepository userRepository,
        IUserIdProvider userIdProvider,
        IFileUploader fileUploader,
        IMmrCalculator mmrCalculator,
        IPlayerRepository playerRepository)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _userIdProvider = userIdProvider;
        _fileUploader = fileUploader;
        _mmrCalculator = mmrCalculator;
        _playerRepository = playerRepository;
    }

    public async Task<Team> CreateTeamAsync(Team team, IFormFile logo)
    {
        var manager = await _userRepository.GetTeamManagerByUserIdAsync(_userIdProvider.UserId!);
        var logoUrl = await _fileUploader.UploadFileAsync(logo);

        team.TeamManager = manager!;
        team.LogoUrl = logoUrl;

        var createdTeam = await _teamRepository.CreateTeamAsync(team);

        return createdTeam;
    }

    public async Task<Team> UpdateTeamAsync(int id, Team updatedTeam, IFormFile logo)
    {
        var team = await _teamRepository.GetTeamByIdAsync(id)
            ?? throw new NotFoundException("Team with this id doesn't exist.");

        if (logo != null)
        {
            await _fileUploader.DeleteFileAsync(team.LogoUrl);
            var newLogoUrl = await _fileUploader.UploadFileAsync(logo);
            team.LogoUrl = newLogoUrl;
        }

        team.Name = updatedTeam.Name ?? team.Name;
        team.Description = updatedTeam.Description ?? team.Description;

        return await _teamRepository.UpdateTeamAsync(team);
    }

    public async Task DeactivateTeamAsync(int id)
    {
        var team = await _teamRepository.GetTeamByIdAsync(id)
            ?? throw new NotFoundException("Team with this id doesn't exist.");

        team.Deleted = true;

        team.Players.ForEach(p => p.Team = null);

        await _teamRepository.UpdateTeamAsync(team);
    }

    public async Task<Team> GetTeamByIdAsync(int id)
    {
        return await _teamRepository.GetTeamByIdAsync(id)
            ?? throw new NotFoundException("Team with this id was not found.");
    }

    public async Task<List<Team>> GetAllTeamsAsync(string? userId = null)
    {
        if (userId == null)
        {
            return await _teamRepository.GetAllTeamsAsync();
        }

        var teamManager = await _userRepository.GetTeamManagerByUserIdAsync(userId)
            ?? throw new NotFoundException("Team manager with this id does not exist.");

        return teamManager.Teams;
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        var teams = await _teamRepository.GetAllTeamsAsync();

        return teams;
    }

    public async Task<Team> GetTeamByTeamManagerId(string teamManagerUserId)
    {
        var team = await _teamRepository.GetTeamByTeamManagerUserId(teamManagerUserId)
            ?? throw new NotFoundException("Team with this id was not found.");

        return team;
    }

    public async Task UpdateTeamPlayersMmrAsync(Team winner, Team loser, int matchId)
    {
        var losingTeamMmr = (int)loser!.Players.Average(p => p.Mmr);
        var winningTeamMmr = (int)winner!.Players.Average(p => p.Mmr);

        foreach (var player in winner!.Players)
        {
            var matchPlayerScore = player.Scores.Single(s => s.Match.Id == matchId);
            player.Mmr = _mmrCalculator.RecalculatePlayerMmr(player, losingTeamMmr, matchPlayerScore, 1);
            await _playerRepository.UpdatePlayerAsync(player);
        }

        foreach (var player in loser!.Players)
        {
            var matchPlayerScore = player.Scores.Single(s => s.Match.Id == matchId);
            player.Mmr = _mmrCalculator.RecalculatePlayerMmr(player, winningTeamMmr, matchPlayerScore, 0);
            await _playerRepository.UpdatePlayerAsync(player);
        }
    }
}
