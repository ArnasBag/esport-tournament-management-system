using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Host.Models;

public class InvitationResponse
{
    public int Id { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public PlayerResponse Receiver { get; set; }
    public TeamManagerResponse Sender { get; set; }
    public TeamResponse Team { get; set; }
}
