using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hackathon2024API.Data.Models
{
    public class User : IdentityUser<long>
    {
        public short MaxFilesCount { get; set; }
        
        public bool CanUpload { get; set; }
        
        public bool CanRead { get; set; }

        [Range(0,100)]
        public byte ImageQuality { get; set; }
        public bool MandatoryEncryption { get; set; }
        //public bool CanChange { get; set; }

        public bool CanDelete { get; set; }
        public long MaxFileSizeMb { get; set; }

        public virtual ICollection<UserFile> Files { get; set; }
        public virtual ICollection<FileExtention> FileExtention { get; set; }
        
    }
}
