namespace ESTMS.API.DataAccess.Entities;

public class PlayerScore
{
    public int Id { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public Player Player { get; set; }
    public Match Match { get; set; }
}
