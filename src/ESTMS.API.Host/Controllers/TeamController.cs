﻿using AutoMapper;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;
using ESTMS.API.Host.Models.Misc;
using ESTMS.API.Host.Models.Teams;
using ESTMS.API.Services.Matches;
using ESTMS.API.Services.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("teams")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IInvitationService _invitationService;
    private readonly IPlayerScoreService _playerScoreService;
    private readonly IMapper _mapper;

    public TeamController(ITeamService teamService, IMapper mapper,
        IInvitationService invitationService, IPlayerScoreService playerScoreService)
    {
        _teamService = teamService;
        _mapper = mapper;
        _invitationService = invitationService;
        _playerScoreService = playerScoreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeams()
    {
        var teams = await _teamService.GetAllTeamsAsync();

        return Ok(_mapper.Map<List<TeamResponse>>(teams));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeamById(int id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);

        return Ok(_mapper.Map<TeamResponse>(team));
    }

    [HttpPost]
    [Authorize(Roles = Roles.TeamManager)]
    public async Task<IActionResult> CreateTeam([FromForm] CreateTeamRequest request)
    {
        var team = _mapper.Map<Team>(request);

        var createdTeam = await _teamService.CreateTeamAsync(team, request.Logo);

        return Created("/teams", _mapper.Map<TeamResponse>(createdTeam));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.TeamManager)]
    public async Task<IActionResult> UpdateTeam(int id, [FromForm] UpdateTeamRequest request)
    {
        var team = await _teamService.UpdateTeamAsync(id, _mapper.Map<Team>(request), request.Logo);

        return Ok(_mapper.Map<TeamResponse>(team));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.TeamManager)]
    public async Task<IActionResult> DeactivateTeam(int id)
    {
        await _teamService.DeactivateTeamAsync(id);

        return NoContent();
    }

    [HttpPost("{id}/invitations")]
    [Authorize(Roles = Roles.Player)]
    public async Task<IActionResult> RequestTeamInvite(int id)
    {
        var invitation = await _invitationService.CreateInviteForTeamAsync(id);

        return Created("/test", _mapper.Map<InvitationResponse>(invitation));
    }

    [HttpGet("{id}/kda")]
    public async Task<IActionResult> GetTeamKda(int id)
    {
        var kda = await _playerScoreService.GetTeamKdaAsync(id);

        return Ok(new KdaResponse { Kda = kda});
    }

    [HttpGet("{id}/player-scores")]
    public async Task<IActionResult> GetTeamPlayerScores(int id, [FromQuery] DateFilterQueryParams dateFilterQueryParams)
    {
        var playerScores = await _playerScoreService.GetPlayerScoresByTeamId(
            id, dateFilterQueryParams.From, dateFilterQueryParams.To);

        return Ok(playerScores);
    }
}
