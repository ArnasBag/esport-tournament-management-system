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
    public DbSet<Rank> Ranks { get; set; }

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

        builder.Entity<Rank>()
            .HasData(Enum.GetValues(typeof(RankEnum))
                .Cast<RankEnum>()
                .Select(e => new Rank
                {
                    Id = (short)e,
                    Name = e.ToString()
                }));

        base.OnModelCreating(builder);
    }
}