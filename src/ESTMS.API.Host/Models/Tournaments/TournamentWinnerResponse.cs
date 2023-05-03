namespace ESTMS.API.Host.Models.Tournament;

public class TournamentWinnerResponse
{
    public int Id { get; set; }
    public TeamResponse? WinnerTeam { get; set; }
}