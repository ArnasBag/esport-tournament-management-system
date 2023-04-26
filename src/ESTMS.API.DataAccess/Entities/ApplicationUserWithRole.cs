namespace ESTMS.API.DataAccess.Entities;

public class ApplicationUserWithRole : ApplicationUser
{
    public string Role { get; set; }
}
