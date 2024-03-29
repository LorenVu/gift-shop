﻿using GiftShop.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace GiftShop.Infastructure.DataAccess;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Image> ProductImages { get; set; }
    public DbSet<Property> ProductProperties { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> OrderItems { get; set; }
    public DbSet<Status> OrderStatus { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserLog> UserLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Friends)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.FriendID)
            .HasPrincipalKey(u => u.Id)
            .IsRequired();

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
