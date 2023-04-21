using ESTMS.API.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<TeamManager> TeamManagers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Invitation>()
            .HasOne(i => i.Receiver)
            .WithMany(r => r.ReceivedInvitations);

        builder.Entity<Invitation>()
            .HasOne(i => i.Sender)
            .WithMany(r => r.SentInvitations);

        builder.Entity<ApplicationUser>()
            .UseTptMappingStrategy();

        builder.Entity<Team>()
            .HasQueryFilter(t => !t.Deleted);

        base.OnModelCreating(builder);
    }
}