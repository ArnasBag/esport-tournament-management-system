using System.ComponentModel.DataAnnotations;

namespace ESTMS.API.Host.Models.Auth;

public class RegistrationRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
