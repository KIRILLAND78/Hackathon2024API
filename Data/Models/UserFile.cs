namespace Hackathon2024API.Data.Models
{
    public class UserFile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DiskLocation { get; set; }
        public bool Encrypted { get; set; }
        public virtual User Owner { get; set; }
        public long OwnerId { get; set; }
    }
}
