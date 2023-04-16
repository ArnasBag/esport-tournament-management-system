using ESTMS.API.Host.Models;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("invitations")]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeInvitationStatus(int id, InvitationStatusRequest request)
    {
        await _invitationService.ChangeInvitationStatusAsync(id, request.Status);
        return NoContent();
    }
}
