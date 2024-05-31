using Hackathon2024API.Data.Models;
using Hackathon2024API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;
using log4net;
using log4net.Config;

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
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasData(new User { UserName = "Admin User", Email= "admin@mail.ru", Id=1 });
            builder.Entity<IdentityRole<long>>().HasData(
                new IdentityRole<long> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<long> { Id = 2, Name = "User", NormalizedName = "USER" }
            );        }
    }
}
