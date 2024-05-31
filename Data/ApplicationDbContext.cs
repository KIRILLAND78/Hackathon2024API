﻿using Hackathon2024API.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Hackathon2024API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(new Role { Name = "Admin", Id=1 });
            builder.Entity<User>().HasData(new User { Name = "Admin User", Mail="admin@mail.ru", Id=1 });
            builder.Entity<User>()
                .HasMany(p => p.Roles)
                .WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                je => {
                    je.HasKey("RoleId", "UserId");
                    je.HasData(
            new { RoleId = 1l, UserId = 1l }
            );
            });
        }
    }
}
