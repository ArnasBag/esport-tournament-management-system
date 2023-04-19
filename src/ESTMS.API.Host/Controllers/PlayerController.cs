using AutoMapper;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;
using ESTMS.API.Host.Models.Player;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, UpdatePlayerRequest request)
    {
        //TODO
        // Auto mapper has problems mapping to inherit class properties
        // lets leave like this
        var updatedPlayer = new Player
        {
            ApplicationUser = new ApplicationUser
            {
                UserName = request.UserInfo.Username,
                NormalizedUserName = request.UserInfo.Username.ToUpper(),
            },
            PicturePath = request.PicturePath,
            AboutMeText = request.AboutMeText,
            Team = new Team
            {
                Id = request.TeamId
            }
        };

        var player = await _playerService.UpdatePlayerAsync(id, updatedPlayer);

        return Ok(_mapper.Map<PlayerResponse>(player));
    }

    [HttpPut("{id}/point")]
    //[Authorize(Roles = Roles.Player)]
    public async Task<IActionResult> UpdatePlayerPoint(int id, [FromBody] UpdatePlayerPointRequest request)
    {
        var player = await _playerService.UpdatePlayersPointAsync(id, request.Points);

        return Ok(_mapper.Map<PlayerResponse>(player));
    }
}