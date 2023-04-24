namespace ESTMS.API.Host.Models.Tournament;

public class TeamResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public UserResponse TeamManager { get; set; } = new();
}