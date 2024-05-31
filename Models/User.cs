namespace Hackathon2024API.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public virtual ICollection<UserFile> Files { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
