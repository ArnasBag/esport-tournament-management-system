﻿using ESTMS.API.DataAccess.Entities;
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
            .WithMany(r => r.Invitations);

        builder.Entity<ApplicationUser>()
            .UseTptMappingStrategy();

        builder.Entity<Team>()
            .HasQueryFilter(t => !t.Deleted);

        builder.Entity<Match>()
            .HasMany(t => t.Competitors)
            .WithMany(m => m.Matches);
        
        builder.Entity<Tournament>()
            .HasMany(m => m.Matches)
            .WithOne(t => t.Tournament);


        base.OnModelCreating(builder);
    }
}