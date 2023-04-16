using Microsoft.AspNetCore.Identity;

namespace ESTMS.API.DataAccess.Entities;

public class ApplicationUser : IdentityUser
{
    public bool Active { get; set; } = true;
    public List<Invitation> Invitations { get; set; }  
}
