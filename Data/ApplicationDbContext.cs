using Hackathon2024API.Data.Models;
using Hackathon2024API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hackathon2024API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<UserFile> UserFiles { get; set; }
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>().HasData(new Role { Title = "Admin", Id=1 });
            builder.Entity<User>().HasData(new User { UserName = "Admin User", Email= "admin@mail.ru", Id=1 });
           
        }
    }
}
