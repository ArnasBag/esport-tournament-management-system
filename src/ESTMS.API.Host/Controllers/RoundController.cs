using AutoMapper;
using ESTMS.API.Host.Models;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("rounds")]
public class RoundController : ControllerBase
{
    private readonly IRoundService _roundService;
    private readonly IMapper _mapper;

    public RoundController(IRoundService roundService, IMapper mapper)
    {
        _roundService = roundService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoundById(int id)
    {
        var round = await _roundService.GetRoundByIdAsync(id);

        return Ok(_mapper.Map<RoundResponse>(round));
    }
}