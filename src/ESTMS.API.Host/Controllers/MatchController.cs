using AutoMapper;
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatchStatus(int id, MatchStatusRequest request)
    {
        var match = await _matchService.UpdateMatchStatusAsync(id, request.Status);

        return Ok();
    }
}
