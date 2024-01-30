using GiftShop.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.Infastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserLog> UserLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.UniqueDisplayId)
            .IsUnique();

        builder.Entity<UserLog>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<UserLog>()
            .HasIndex(b => b.UserId);

        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
    }
}
