using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("teams")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IInvitationService _invitationService;
    private readonly IMapper _mapper;

    public TeamController(ITeamService teamService, IMapper mapper, 
        IInvitationService invitationService)
    {
        _teamService = teamService;
        _mapper = mapper;
        _invitationService = invitationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam(CreateTeamRequest request)
    {
        var team = _mapper.Map<Team>(request);

        var createdTeam = await _teamService.CreateTeamAsync(team);

        return Created("/test", _mapper.Map<TeamResponse>(createdTeam));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(int id, UpdateTeamRequest request)
    {
        var team = await _teamService.UpdateTeamAsync(id, _mapper.Map<Team>(request));

        return Ok(_mapper.Map<TeamResponse>(team));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeactivateTeam(int id)
    {
        await _teamService.DeactivateTeamAsync(id);

        return NoContent();
    }

    [HttpPost("{id}/invitations")]
    public async Task<IActionResult> InvitePlayerToTeam(int id, CreateInvitationRequest request)
    {
        var invitation = await _invitationService.CreateInvitationAsync(id, request.UserId);

        return Created("/test", _mapper.Map<InvitationResponse>(invitation));
    }
}
