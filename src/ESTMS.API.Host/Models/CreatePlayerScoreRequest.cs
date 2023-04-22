namespace ESTMS.API.Host.Models;

public class CreatePlayerScoreRequest
{
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public int MatchId { get; set; }
}
