﻿namespace ESTMS.API.DataAccess.Entities;

public class Player
{
    public int Id { get; set; }
    public List<Invitation> Invitations { get; set; }
    public Team? Team { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}