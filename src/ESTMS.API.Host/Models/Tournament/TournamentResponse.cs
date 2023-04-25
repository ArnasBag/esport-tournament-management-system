using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Host.Models.Tournament;

public class TournamentResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TournamentWinnerResponse? Winner { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public List<TeamResponse>? Teams { get; set; }
    public List<RoundResponse>? Rounds { get; set; }
    public TournamentManagerResponse Manager { get; set; }
}