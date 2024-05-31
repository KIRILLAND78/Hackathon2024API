using Hackathon2024API.Data;
using Hackathon2024API.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Hackathon2024API.Controllers
{
    public static class IFormFileExtensions
    {
        public static string GetHash(this IFormFile formFile)
        {
            using (var stream = formFile.OpenReadStream())
            {
                MemoryStream mst = new MemoryStream();
                stream.CopyTo(mst);

                if (mst.ToArray() == null || mst.ToArray().Length == 0) return "";

                using (var md5 = MD5.Create())
                {
                    return string.Join("", md5.ComputeHash(mst.ToArray()).Select(x => x.ToString("X2")));
                }
            }
        }
    }



    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FileController : ControllerBase
    {
        ApplicationDbContext _context;
        public FileController(ApplicationDbContext context) {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index() {
            return Ok(_context.UserFiles.ToList());
        }
        
        
        [HttpGet("MyFiles")]
        public async Task<ActionResult> MyFiles()
        {
            return Ok(_context.UserFiles.Where(x=>x.Id==1).ToList());
        }
        
        
        [HttpPut]
        public async Task<ActionResult> Upload([FromServices] EncryptionService encryption, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var hash = file.GetHash();

                if (_context.UserFiles.Any(file => file.DiskLocation == hash))
                {
                    continue;
                }

                using (var stream = file.OpenReadStream())
                {
                    Directory.CreateDirectory("UserFiles\\1");
                        stream.Seek(0, SeekOrigin.Begin);

                        using (var fileStream = new FileStream($"UserFiles\\1\\{hash}", FileMode.OpenOrCreate))
                        {
                            encryption.EncryptFile(stream, fileStream, file.FileName);
                        }

                }
                
                await _context.UserFiles.AddAsync(new Models.UserFile { DiskLocation = $"{hash}", Name = file.FileName, Owner = _context.Users.First() });
            }
            await _context.SaveChangesAsync();
            return Ok(_context.UserFiles.Where(x => x.Id == 1).ToList());
        }
        
        
        [HttpGet("download")]
        public async Task<ActionResult> Download([FromServices] EncryptionService encryption, string fileName)
        {
            var file = _context.UserFiles.FirstOrDefault(x => x.Name==fileName);
            if (file == null) return NotFound();
            try
            {
                MemoryStream decryptedStream = new MemoryStream();
                {
                    using (FileStream fileStream = new FileStream($"UserFiles\\1\\{file.DiskLocation}", FileMode.Open))
                    {
                        encryption.DecryptFile(fileStream, decryptedStream, file.Name);
                        decryptedStream.Seek(0, SeekOrigin.Begin);
                        return File(decryptedStream, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
                        
                    }
                }
            } catch (FileNotFoundException ex)
            {
                _context.Remove(file);
                await _context.SaveChangesAsync();
                throw new Exception("file corrupted");
            }
            catch (Exception ex)
            {
                return Ok(123);
            }
        }
        /// <summary>
        /// �������� �����
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(string fileName)
        {
            var file = _context.UserFiles.FirstOrDefault(x => x.Name == fileName);
            
            if (file == null) return NotFound();
            
            System.IO.File.Delete($"UserFiles\\1\\{file.DiskLocation}");
            _context.Remove(file);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}