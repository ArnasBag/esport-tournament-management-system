namespace ESTMS.API.Host.Models.Players;

public class CreatePlayerRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AboutMeText { get; set; }
    public IFormFile ProfilePicture { get; set; }
}