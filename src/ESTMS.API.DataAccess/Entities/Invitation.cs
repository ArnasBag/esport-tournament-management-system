namespace ESTMS.API.DataAccess.Entities;

public class Invitation
{
    public int Id { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Player Receiver { get; set; }
    public TeamManager Sender { get; set; }
    public Team Team { get; set; }
}

public enum InvitationStatus
{
    Accepted,
    Declined,
    Pending
}
