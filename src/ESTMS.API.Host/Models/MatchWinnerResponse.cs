namespace ESTMS.API.Host.Models;

public class MatchWinnerResponse
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int WinnerTeamId { get; set; }
}
