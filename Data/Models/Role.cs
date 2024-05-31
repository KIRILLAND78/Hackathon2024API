using Hackathon2024API.Models;

namespace Hackathon2024API.Data.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string Title { get; set; }
        
        
        public ICollection<User> Users { get; set; }
        
        public ICollection<FileExtention> AvailableExtentions { get; set; }
    }
}
