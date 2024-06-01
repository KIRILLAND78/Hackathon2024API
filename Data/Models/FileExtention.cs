namespace Hackathon2024API.Data.Models
{
	public class FileExtention
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public virtual ICollection<User> Users { get; set; }
	}
}
