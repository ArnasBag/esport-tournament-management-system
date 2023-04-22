using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Host.Models.Tournament;

public class TournamentResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TournamentWinner? Winner { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public List<Team>? Teams { get; set; }
    public List<Match>? Matches { get; set; }
}