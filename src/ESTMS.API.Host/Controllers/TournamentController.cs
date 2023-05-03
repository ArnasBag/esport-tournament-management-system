using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models.Tournament;
using ESTMS.API.Host.Models.Tournaments;
using ESTMS.API.Services.Tournaments;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("tournaments")]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IRoundService _roundService;
    private readonly IMapper _mapper;

    public TournamentController(ITournamentService tournamentService, 
        IMapper mapper, IRoundService roundService)
    {
        _tournamentService = tournamentService;
        _mapper = mapper;
        _roundService = roundService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTournaments(string? tournamentManagerId)
    {
        var tournaments = await _tournamentService.GetAllTournamentsAsync(tournamentManagerId);

        return Ok(_mapper.Map<List<TournamentResponse>>(tournaments));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTournamentById(int id)
    {
        var tournaments = await _tournamentService.GetTournamentByIdAsync(id);

        return Ok(_mapper.Map<TournamentResponse>(tournaments));
    }

    [HttpGet("{id}/rounds")]
    public async Task<IActionResult> GetTournamentRounds(int id)
    {
        var rounds = await _roundService.GetTournamentRounds(id);

        return Ok(_mapper.Map<List<RoundResponse>>(rounds));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTournament(CreateTournamentRequest request)
    {
        var tournament = _mapper.Map<Tournament>(request);

        var createdTournament = await _tournamentService.CreateTournamentAsync(tournament);

        return Ok(_mapper.Map<TournamentResponse>(createdTournament));
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
        var tournament = await _tournamentService.CreateBracket(id);

        return Ok(_mapper.Map<TournamentResponse>(tournament));
    }

    [HttpPost("{id}/tournament-teams")]
    public async Task<IActionResult> JoinTournament(int id, JoinTournamentRequest request)
    {
        await _tournamentService.JoinTournamentAsync(id, request.TeamId);
        return Ok();
    }

    [HttpDelete("{id}/tournament-teams")]
    public async Task<IActionResult> LeaveTournament(int id, LeaveTournamentRequest request)
    {
        await _tournamentService.LeaveTournamentAsync(id, request.TeamId);
        return Ok();
    }
}