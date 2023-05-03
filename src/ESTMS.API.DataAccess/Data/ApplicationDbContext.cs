using ESTMS.API.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<TeamManager> TeamManagers { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TournamentWinner> TournamentWinners { get; set; }
    public DbSet<TournamentManager> TournamentManagers { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchWinner> MatchWinners { get; set; }
    public DbSet<PlayerScore> PlayerScores { get; set; }

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

        builder.Entity<MatchWinner>()
            .Ignore(mw => mw.Match);
        
        builder.Entity<Tournament>()
            .HasMany(m => m.Rounds)
            .WithOne(t => t.Tournament);

        builder.Entity<Round>()
            .HasMany(m => m.Matches)
            .WithOne(r => r.Round);

        base.OnModelCreating(builder);
    }
}