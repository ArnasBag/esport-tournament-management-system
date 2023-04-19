using AutoMapper;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.Host.Models;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Authorize(Roles = Roles.Player)]
[Route("invitations")]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;
    private readonly IMapper _mapper;

    public InvitationController(IInvitationService invitationService, 
        IMapper mapper)
    {
        _invitationService = invitationService;
        _mapper = mapper;
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeInvitationStatus(int id, InvitationStatusRequest request)
    {
        await _invitationService.ChangeInvitationStatusAsync(id, request.Status);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInvitations()
    {
        var invitations = await _invitationService.GetAllInitationsAsync();

        return Ok(_mapper.Map<List<InvitationResponse>>(invitations));
    }
}
