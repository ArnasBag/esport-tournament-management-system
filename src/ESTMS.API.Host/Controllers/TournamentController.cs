using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;
using ESTMS.API.Host.Models.Tournament;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("tournaments")]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IMapper _mapper;

    public TournamentController(ITournamentService tournamentService, IMapper mapper)
    {
        _tournamentService = tournamentService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTournaments()
    {
        var players = await _tournamentService.GetAllTournamentsAsync();

        return Ok(_mapper.Map<List<TournamentResponse>>(players));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTournamentById(int id)
    {
        var player = await _tournamentService.GetTournamentByIdAsync(id);

        return Ok(_mapper.Map<TournamentResponse>(player));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTournament(CreateTournamentRequest request)
    {
        var tournament = _mapper.Map<Tournament>(request);

        var player = await _tournamentService.CreateTournamentAsync(tournament);

        return Ok(_mapper.Map<TournamentResponse>(player));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTournament(int id, UpdateTournamentRequest request)
    {
        var updatedTournament = _mapper.Map<Tournament>(request);

        var tournament = await _tournamentService.UpdateTournamentAsync(id, updatedTournament);

        return Ok(_mapper.Map<TournamentResponse>(tournament));
    }

    [HttpPut("{id}/winner")]
    public async Task<IActionResult> UpdateTournamentWinner(int id, UpdateTournamentWinnerRequest request)
    {
        var updatedWinner = request.Id;

        var tournament = await _tournamentService.UpdateTournamentWinnerAsync(id, updatedWinner);

        return Ok(_mapper.Map<TournamentResponse>(tournament));
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTournamentStatus(int id, UpdateTournamentStatusRequest request)
    {
        var updatedStatus = Enum.Parse<Status>(request.Status);

        var tournament = await _tournamentService.UpdateTournamentStatusAsync(id, updatedStatus);

        return Ok(_mapper.Map<TournamentResponse>(tournament));
    }

    [HttpPut("{id}/bracket")]
    public async Task<IActionResult> CreateMatches(int id)
    {
        return Ok(Task.FromResult(Empty));
    }
}