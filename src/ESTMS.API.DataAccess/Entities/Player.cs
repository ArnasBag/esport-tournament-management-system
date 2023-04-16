namespace ESTMS.API.DataAccess.Entities;

public class Player : ApplicationUser
{
    public List<Invitation> Invitations { get; set; }
}
