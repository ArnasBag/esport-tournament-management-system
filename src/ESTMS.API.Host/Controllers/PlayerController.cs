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
    private readonly IPlayerScoreService _playerScoreService;
    private readonly IInvitationService _invitationService;
    private readonly IMapper _mapper;

    public PlayerController(IPlayerService playerService, IMapper mapper,
        IInvitationService invitationService, IPlayerScoreService playerScoreService)
    {
        _playerService = playerService;
        _mapper = mapper;
        _invitationService = invitationService;
        _playerScoreService = playerScoreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayers()
    {
        var players = await _playerService.GetAllPlayersAsync();

        return Ok(_mapper.Map<List<PlayerResponse>>(players));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerById(string id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);

        return Ok(_mapper.Map<PlayerResponse>(player));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(string id, UpdatePlayerRequest request)
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
        };

        var player = await _playerService.UpdatePlayerAsync(id, updatedPlayer);

        return Ok(_mapper.Map<PlayerResponse>(player));
    }

    [HttpPut("{id}/point")]
    //[Authorize(Roles = Roles.Player)]
    public async Task<IActionResult> UpdatePlayerPoint(string id, [FromBody] UpdatePlayerPointRequest request)
    {
        var player = await _playerService.UpdatePlayersPointAsync(id, request.Points);

        return Ok(_mapper.Map<PlayerResponse>(player));
    }

    [HttpPost("{id}/invitations")]
    [Authorize(Roles = Roles.TeamManager)]
    public async Task<IActionResult> InvitePlayerToTeam(string id, CreateInvitationRequest request)
    {
        var invitation = await _invitationService.CreateInviteForPlayerAsync(id, request.TeamId);

        return Created("/test", _mapper.Map<InvitationResponse>(invitation));
    }

    [HttpPost("{id}/player-scores")]
    public async Task<IActionResult> CreatePlayerScore(string id, CreatePlayerScoreRequest request)
    {
        var playerScore = await _playerScoreService.CreatePlayerScoreAsync(id, request.MatchId, _mapper.Map<PlayerScore>(request));

        return Ok(_mapper.Map<CreatePlayerScoreRequest>(playerScore));
    }

    [HttpGet("{id}/player-scores")]
    public async Task<IActionResult> GetPlayerScores(string id)
    {
        var playerScores = await _playerScoreService.GetPlayerScoresByUserId(id);

        return Ok(_mapper.Map<List<PlayerScoreResponse>>(playerScores));
    }
}