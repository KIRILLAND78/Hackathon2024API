using Hackathon2024API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Hackathon2024API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<UserFile> UserFiles { get; set; }
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>().HasData(new Role { Title = "Admin", Id=1 });
            builder.Entity<User>().HasData(new User { UserName = "Admin User", Email= "admin@mail.ru", Id=1 });
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
