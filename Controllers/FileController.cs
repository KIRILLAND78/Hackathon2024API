using Hackathon2024API.Data;
using Hackathon2024API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using log4net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
    //[Authorize]
    public class FileController : ControllerBase
    {
        private static ILog log;

        ApplicationDbContext _context;
        public FileController(ApplicationDbContext context, ILog logger) {
            _context = context;
            log = logger;
        }
        /// <summary>
        /// ���������� ������ ���� ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index() {
            return Ok(_context.UserFiles.ToList());
        }
        /// <summary>
        /// ���������� ������ ������ ������������
        /// </summary>
        /// <returns></returns>
        [HttpGet("MyFiles")]
        public async Task<ActionResult> MyFiles()
        {
            return Ok(_context.UserFiles.Where(x=>x.Id==1).ToList());
        }
        /// <summary>
        /// �������� �����
        /// </summary>
        /// <returns></returns>
        [HttpPut]
		public async Task<ActionResult> Upload([FromServices] EncryptionService encryption, List<IFormFile> files)
		{
			try
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

					_context.UserFiles.Add(new Models.UserFile { DiskLocation = $"{hash}", Name = file.FileName, Owner = _context.Users.First() });
				}
				_context.SaveChanges();
				log.Info($"Загружено {files.Count} файлов");
				return Ok(_context.UserFiles.Where(x => x.Id == 1).ToList());
			}
			catch (Exception ex)
			{
				log.Error($"Ошибка при загрузке файла: {ex.Message}");
				return Problem();
			}
		}
		/// <summary>
		/// ���������� �����
		/// </summary>
		/// <returns></returns>
		[HttpGet("download")]
		public async Task<ActionResult> Download([FromServices] EncryptionService encryption, string fileName)
		{
			try
			{
				log.Info($"Запрос на скачивание файла {fileName}");
				var file = _context.UserFiles.Where(x => x.Name == fileName).FirstOrDefault();
				if (file == null) return NotFound();

				MemoryStream decryptedStream = new MemoryStream();
				{
					using (FileStream fileStream = new FileStream($"UserFiles\\1\\{file.DiskLocation}", FileMode.Open))
					{
						encryption.DecryptFile(fileStream, decryptedStream, file.Name);
						decryptedStream.Seek(0, SeekOrigin.Begin);
						log.Info($"Файл {fileName} успешно скачан");
						return File(decryptedStream, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);

					}
				}
			}
			catch (FileNotFoundException ex)
			{
				log.Error($"Файл {fileName} не найден: {ex.Message}");
				var file = _context.UserFiles.Where(x => x.Name == fileName).FirstOrDefault();
				if (file != null)
				{
					_context.Remove(file);
					_context.SaveChanges();
				}
				throw new Exception("file corrupted");
			}
			catch (Exception ex)
			{
				log.Error($"Ошибка при скачивании файла {fileName}: {ex.Message}");
				return Problem("Error");
			}
		}
		/// <summary>
		/// �������� �����
		/// </summary>
		/// <returns></returns>
		[HttpDelete("delete")]
		public async Task<ActionResult> Delete(string fileName)
		{
			try
			{
				log.Info($"Запрос на удаление файла {fileName}");
				var file = _context.UserFiles.Where(x => x.Name == fileName).FirstOrDefault();
				if (file == null)
                    return NotFound();
				System.IO.File.Delete($"UserFiles\\1\\{file.DiskLocation}");
				_context.Remove(file);
				_context.SaveChanges();
				log.Info($"Файл {fileName} успешно удален");
				return Ok();
			}
			catch (Exception ex)
			{
				log.Error($"Ошибка при удалении файла {fileName}: {ex.Message}");
				return Problem("Error");
			}
		}
	}
}