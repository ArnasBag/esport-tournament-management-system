using AutoMapper;
using ESTMS.API.Host.Models;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("team-manager")]
public class TeamManagerController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IMapper _mapper;

    public TeamManagerController(ITeamService teamService, IMapper mapper)
    {
        _teamService = teamService;
        _mapper = mapper;
    }

    [HttpGet("{id}/team")]
    public async Task<IActionResult> GetTeamManagerTeam(string id)
    {
        var team = await _teamService.GetTeamByTeamManagerId(id);

        return Ok(_mapper.Map<TeamResponse>(team));
    }
}
