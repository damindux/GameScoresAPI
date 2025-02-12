using GameScoresApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameScoresApi.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Score> Scores { get; set; }
    public DbSet<Player> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Score>()
            .HasOne(s => s.Player)
            .WithMany()
            .HasForeignKey(s => s.PlayerId);

        builder.Entity<Score>()
            .Property(s => s.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");
        
        builder.Entity<Score>()
            .Property(s => s.ModifiedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");
    }
}