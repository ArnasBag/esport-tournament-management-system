namespace ESTMS.API.Host.Models.Users;

public class UserResponse
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Active { get; set; }
    public string Role { get; set; }
}
