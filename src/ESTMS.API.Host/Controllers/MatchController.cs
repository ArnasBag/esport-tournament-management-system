using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("matches")]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly IMapper _mapper;

    public MatchController(IMatchService matchService, IMapper mapper)
    {
        _matchService = matchService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatchById(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);

        return Ok(_mapper.Map<MatchResponse>(match));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatchStatus(int id, MatchStatusRequest request)
    {
        var match = await _matchService.UpdateMatchStatusAsync(id, request.Status);

        return Ok(_mapper.Map<MatchResponse>(match));
    }

    [HttpPut("{id}/winner")]
    public async Task<IActionResult> UpdateMatchWinner(int id, CreateMatchWinnerTeamRequest request)
    {
        var match = await _matchService.UpdateMatchWinnerAsync(id, request.TeamId);

        return Ok(_mapper.Map<MatchResponse>(match));
    }

    [HttpPut("{id}/date")]
    public async Task<IActionResult> UpdateMatchDate(int id, UpdateMatchDateRequest request)
    {
        var match = await _matchService.UpdateMatchDateAsync(id, _mapper.Map<Match>(request));

        return Ok(_mapper.Map<MatchResponse>(match));
    }
}
