using Hackathon2024API.Data.Models;

namespace Hackathon2024API.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public short MaxFilesCount { get; set; }
        public bool CanUpload { get; set; }
        public bool CanRead { get; set; }
        public bool CanChange { get; set; }
        public bool CanDelete { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<FileExtention> AvailableExtentions { get; set; }
    }
}
