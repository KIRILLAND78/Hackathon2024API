using Hackathon2024API.Data;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Cryptography;

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
    public class FileController : ControllerBase
    {
        ApplicationDbContext _context;
        public FileController(ApplicationDbContext context) {
            _context = context;
        }
        /// <summary>
        /// Возвращает список всех файлов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index() {
            return Ok(_context.UserFiles.ToList());
        }
        /// <summary>
        /// Возвращает список файлов пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("MyFiles")]
        public async Task<ActionResult> MyFiles()
        {
            return Ok(_context.UserFiles.Where(x=>x.Id==1).ToList());
        }
        /// <summary>
        /// Загрузка файла
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Upload(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var hash = file.GetHash();
                using (var stream = file.OpenReadStream())
                {
                    Directory.CreateDirectory("UserFiles\\1");
                    using (var fileStream = new FileStream($"UserFiles\\1\\{hash}", FileMode.OpenOrCreate))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }

                    _context.UserFiles.Add(new Models.UserFile { DiskLocation = $"{hash}", Name = file.FileName, Owner = _context.Users.First() });
            }
            _context.SaveChanges();
            return Ok(_context.UserFiles.Where(x => x.Id == 1).ToList());
        }
        /// <summary>
        /// Скачивание файла
        /// </summary>
        /// <returns></returns>
        [HttpGet("download")]
        public async Task<ActionResult> Download(string fileName)
        {
            var file = _context.UserFiles.Where(x=>x.Name==fileName).FirstOrDefault();
            if (file == null) return NotFound();
            try
            {
                return Ok(new FileStream($"UserFiles\\1\\{file.DiskLocation}", FileMode.Open));
            } catch (FileNotFoundException ex)
            {
                _context.Remove(file);
                _context.SaveChanges();
                throw new Exception("file corrupted");
            }
        }
        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(string fileName)
        {
            var file = _context.UserFiles.Where(x => x.Name == fileName).FirstOrDefault();
            if (file == null) return NotFound();
            System.IO.File.Delete($"UserFiles\\1\\{file.DiskLocation}");
            _context.Remove(file);
            _context.SaveChanges();
            return Ok();
        }
    }
}