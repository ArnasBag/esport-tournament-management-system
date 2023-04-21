using Microsoft.AspNetCore.Identity;

namespace ESTMS.API.DataAccess.Entities;

public class ApplicationUser : IdentityUser
{
    public bool Active { get; set; } = true;
    public List<Invitation> SentInvitations { get; set; }
    public List<Invitation> ReceivedInvitations{ get; set; }

}
