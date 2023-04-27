namespace ESTMS.API.Host.Models;

public class RoundResponse
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public List<MatchResponse> Matches { get; set; }
}