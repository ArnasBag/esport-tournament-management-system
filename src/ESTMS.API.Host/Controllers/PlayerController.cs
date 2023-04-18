using AutoMapper;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

//TODO add mapping profiles for player DTO's

[ApiController]
[Route("players")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IMapper _mapper;

    public PlayerController(IPlayerService playerService, IMapper mapper)
    {
        _playerService = playerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayers()
    {
        var players = await _playerService.GetAllPlayersAsync();

        return Ok(_mapper.Map<List<PlayerResponse>>(players));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerById(int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);

        return Ok(_mapper.Map<PlayerResponse>(player));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer(CreatePlayerRequest request)
    {
        var player = _mapper.Map<Player>(request);

        var createdPlayer = await _playerService.CreatePlayerAsync(player);

        return Created("/test", _mapper.Map<PlayerResponse>(createdPlayer));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, UpdatePlayerRequest request)
    {
        var player = await _playerService.UpdatePlayerAsync(id, _mapper.Map<Player>(request));

        return Ok(_mapper.Map<PlayerResponse>(player));
    }

    [HttpPut("{id}/rank")]
    //[Authorize(Roles = Roles.Player)]
    public async Task<IActionResult> UpdatePlayerRank(int id)
    {
        var player = await _playerService.UpdatePlayersRankAsync(id);

        return Ok(_mapper.Map<PlayerResponse>(player));
    }
}