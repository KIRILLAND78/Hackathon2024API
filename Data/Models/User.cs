using Hackathon2024API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Hackathon2024API.Models
{
    public class User : IdentityUser<long>
    {
        public long Id { get; set; }
        
        public short MaxFilesCount { get; set; }
        
        public bool CanUpload { get; set; }
        
        public bool CanRead { get; set; }
        
        public bool CanChange { get; set; }
        
        public bool CanDelete { get; set; }
        
        
        public virtual ICollection<UserFile> Files { get; set; }
        
    }
}
