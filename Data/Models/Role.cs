using Microsoft.AspNetCore.Identity;

namespace Hackathon2024API.Data.Models
{
    public class Role : IdentityRole<long>
    {
        public string Title { get; set; }
        
        public ICollection<User> Users { get; set; }
        
        public ICollection<FileExtention> AvailableExtentions { get; set; }
    }
}
