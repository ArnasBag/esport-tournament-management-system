namespace ESTMS.API.Services;

public interface IAuthService
{
    public Task RegisterUserAsync(string username, string email, string password);
    Task<string> LoginUserAsync(string email, string password);
}
