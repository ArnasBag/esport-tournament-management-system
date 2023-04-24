namespace ESTMS.API.DataAccess.Entities;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? LogoUrl { get; set; }
    public bool Deleted { get; set; } = false;
    public TeamManager TeamManager { get; set; }
    public List<Player> Players { get; set; }
    public List<Tournament>? Tournaments { get; set; }
    public List<Match>? Matches { get; set; }
}
