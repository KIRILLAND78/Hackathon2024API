using Microsoft.AspNetCore.Identity;

namespace Hackathon2024API.Models
{
    public class User : IdentityUser<long>
    {
        public long Id { get; set; }
        
        public virtual ICollection<UserFile> Files { get; set; }
        
        public virtual ICollection<Role> Roles { get; set; }
    }
}
