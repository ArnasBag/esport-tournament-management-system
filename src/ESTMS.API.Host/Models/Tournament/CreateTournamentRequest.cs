namespace ESTMS.API.Host.Models.Tournament;

public class CreateTournamentRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int MaxTeamCount { get; set; }
}