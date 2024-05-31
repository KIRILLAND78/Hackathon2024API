namespace Hackathon2024API.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
