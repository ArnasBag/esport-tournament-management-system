namespace ESTMS.API.Host.Models;

public class CreateTeamRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Logo { get; set; }
}
