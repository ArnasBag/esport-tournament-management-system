using ESTMS.API.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace ESTMS.API.Services.Auth;

public interface IAuthService
{
    public Task RegisterUserAsync(string username, string email, string password);
    Task<string> LoginUserAsync(string email, string password);
    Task<(ApplicationUser, string?)> GetMeAsync(string id);
}
