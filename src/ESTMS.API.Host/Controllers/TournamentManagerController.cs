using AutoMapper;
using ESTMS.API.Host.Models;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("tournament-manager")]
public class TournamentManagerController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IMapper _mapper;

    public TournamentManagerController(ITournamentService tournamentService, IMapper mapper)
    {
        _tournamentService = tournamentService;
        _mapper = mapper;
    }

    [HttpGet("{id}/tournament")]
    public async Task<IActionResult> GetTournamentManagerTeam(string id)
    {
        var tournament = await _tournamentService.GetTournamentByTournamentManagerId(id);

        return Ok(_mapper.Map<TeamResponse>(tournament));
    }
}