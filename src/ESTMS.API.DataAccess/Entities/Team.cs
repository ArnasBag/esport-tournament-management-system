namespace ESTMS.API.DataAccess.Entities;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Deleted { get; set; } = false;
    public List<Player> Players { get; set; }
}
