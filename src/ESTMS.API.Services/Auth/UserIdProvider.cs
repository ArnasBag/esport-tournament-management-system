using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ESTMS.API.Services.Auth;

public class UserIdProvider : IUserIdProvider
{
    public string? UserId { get; set; }

    public UserIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
