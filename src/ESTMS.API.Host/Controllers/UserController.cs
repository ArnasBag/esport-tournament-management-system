using AutoMapper;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.Host.Models.Users;
using ESTMS.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("users")]
[EnableCors("AllowVueFrontend")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        return Ok(_mapper.Map<UserResponse>(user));
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCreatedUserCountByDay(DateTime from, DateTime to)
    {
        var userDailyCount = await _userService.GetDailyCreatedUsersAsync(from, to);

        return Ok(userDailyCount);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsersAsync();

        return Ok(_mapper.Map<List<UserResponse>>(users));
    }

    [HttpPut("{id}/activation-status")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ChangeUserActivationStatus(string id, [FromBody] ChangeUserActivationStatusRequest status)
    {
        await _userService.ChangeUserActivityAsync(id, status.ActivationStatus);

        return Ok();
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> ChangeUserRole(string id, [FromBody] ChangeUserRoleRequest role)
    {
        await _userService.ChangeUserRoleAsync(id, role.Role);

        return Ok();
    }
}
