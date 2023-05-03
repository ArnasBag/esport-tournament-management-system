using ESTMS.API.Host.Models.Matches;

namespace ESTMS.API.Host.Models.Tournaments;

public class RoundResponse
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public List<MatchResponse> Matches { get; set; }
}