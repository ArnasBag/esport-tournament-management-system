using ESTMS.API.Host.Models.Player;
using ESTMS.API.Host.Models.Users;
using InvitationStatus = ESTMS.API.DataAccess.Entities.InvitationStatus;

namespace ESTMS.API.Host.Models.Teams;

public class InvitationResponse
{
    public int Id { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserResponse Receiver { get; set; } = new();
    public UserResponse Sender { get; set; } = new();
    public PlayersTeamResponse Team { get; set; }
}
