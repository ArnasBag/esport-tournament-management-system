using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Host.Models.Player;

public class PlayersInvitations
{
    public int Id { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ApplicationUser Sender { get; set; }
    public PlayersTeamResponse Team { get; set; }
}

public enum InvitationStatus
{
    Accepted,
    Declined,
    Pending
}