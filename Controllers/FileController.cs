using Hackathon2024API.Data;
using Hackathon2024API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using log4net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Hackathon2024API.Data.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Hackathon2024API.Data.DTO;
using Microsoft.AspNetCore.Http;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class FileController : ControllerBase
    {
        private static ILog log;

        ApplicationDbContext _context;
        UserManager<User> _userManager;
        User user;
        public FileController(ApplicationDbContext context, ILog logger, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            log = logger;
            _userManager = userManager;
			user = _userManager.FindByEmailAsync(httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value).Result!;
        }


        [HttpGet]
        public async Task<ActionResult> Index([FromQuery] PaginationDTO pg) 
		{
			if (user.CanRead || _userManager.GetRolesAsync(user).Result.Contains("Admin"))
			{
                return Ok(_context.UserFiles.Skip(pg.Count * pg.Page).Take(pg.Count).Select(x=>new { x.Id, x.DiskLocation, x.Encrypted, x.Name, x.OwnerId }).ToList());
            }
            return Unauthorized();
        }


        [HttpGet("MyFiles")]
        public async Task<ActionResult> MyFiles([FromQuery]PaginationDTO pg)
        {
            return Ok(_context.UserFiles.Where(x=>x.Id==user.Id).Skip(pg.Count*pg.Page).Take(pg.Count).Select(x => new { x.Id, x.DiskLocation, x.Encrypted, x.Name, x.OwnerId }).ToList());
        }


        [HttpPut]
		public async Task<ActionResult> Upload([FromServices] EncryptionCompressionService encryption, List<IFormFile> files)
		{
			try
			{
				Dictionary<string, string> results = new();
				foreach (var file in files)
                {
					if (!_userManager.GetRolesAsync(user).Result.Contains("Admin")) {
                    var extension = System.IO.Path.GetExtension(file.FileName);
					if (!_context.FileExtentions.Include(x=>x.Users).Any(x => x.Title == extension && x.Users.Any(z => z.Id == user.Id))){
                        results.Add(file.FileName, "Этот тип файла не разрешен к загрузке");
                        continue;
                    }
                    if (!user.CanUpload)
                    {
                        results.Add(file.FileName, "Нет прав для загрузки файлов");
                        continue;
                    }
					if (user.MaxFilesCount <= _context.UserFiles.Where(x => x.OwnerId == user.Id).Count())
                    {
                        results.Add(file.FileName, "Достигнуто максимальное количество возможных хранимых на сервере файлов");
                        continue;
                    }
                    if (user.MaxFileSizeMb < ((float)file.Length)/1024/1024)
                    {
                        results.Add(file.FileName, "Превышен максимальный размер загружаемого файла");
                        continue;
                    }
                    }

                    string hash = file.GetHash();

                    if (_context.UserFiles.Any(file => file.DiskLocation == hash && file.OwnerId==user.Id))
                    {
                        results.Add(file.FileName, "Файл уже существует");
                        continue;
                    }
                    try
					{
						using (var stream = file.OpenReadStream())
						{
							using (Stream str = new MemoryStream()) {
								stream.CopyTo(str);
							try
                                {
                                    str.Seek(0, SeekOrigin.Begin);
                                    stream.Seek(0, SeekOrigin.Begin);
                                    if (user.ImageQuality < 100 && user.ImageQuality>0)
                                    using (var image = SixLabors.ImageSharp.Image.Load(stream))
                                    {
                                        var encoder = new JpegEncoder
                                        {
                                            Quality = user.ImageQuality
                                        };
                                        image.Save(str, encoder);
                                    }

                            }
							catch (Exception ex)
							{
								//не картинка, ничего, идем дальше
							}
							Directory.CreateDirectory($"UserFiles\\{user.Id}");
                                str.Seek(0, SeekOrigin.Begin);

							using (var fileStream = new FileStream($"UserFiles\\{user.Id}\\{hash}", FileMode.OpenOrCreate))
							{
								if (user.MandatoryEncryption)
									encryption.EncryptCompressFile(str, fileStream, file.FileName);
								else
									encryption.CompressFile(str, fileStream, file.FileName);
							}
                            }
                        }

						_context.UserFiles.Add(new UserFile { DiskLocation = $"{hash}", Encrypted=user.MandatoryEncryption, Name = file.FileName, Owner = user });

                        results.Add(file.FileName, $"Файл успешно {file.FileName} загружен пользователем {user.Id}");
                    } catch(Exception ex)
                    {
                        results.Add(file.FileName, ex.Message);
                    }
				}
				_context.SaveChanges();
				log.Info($"Попытка загрузки {files.Count} файлов");
				return Ok(results);
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
		public async Task<ActionResult> Download([FromServices] EncryptionCompressionService encryption, long id, string fileName)
		{
			try
			{
				log.Info($"Запрос на скачивание файла ({id}: {fileName}) пользователем {user.Id}");

                var file = _context.UserFiles.Where(x => x.Name == fileName && x.OwnerId==id).FirstOrDefault();
				if (file == null) return NotFound();

                if (!(user.CanRead || file.OwnerId==user.Id || _userManager.GetRolesAsync(user).Result.Contains("Admin")))
                {
                    return Unauthorized();
                }
                MemoryStream decryptedStream = new MemoryStream();
				{
					using (FileStream fileStream = new FileStream($"UserFiles\\{id}\\{file.DiskLocation}", FileMode.Open))
					{
						if (file.Encrypted)
						encryption.DecryptDecompressFile(fileStream, decryptedStream, file.Name);
						else encryption.DecompressFile(fileStream, decryptedStream, file.Name);
						decryptedStream.Seek(0, SeekOrigin.Begin);
						log.Info($"Файл ({id}: {fileName}) успешно скачан пользователем {user.Id}");
						return File(decryptedStream, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);

					}
				}
			}
			catch (FileNotFoundException ex)
			{
				log.Error($"Файл ({id}: {fileName}) не найден: {ex.Message}");
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
				log.Error($"Ошибка при скачивании файла ({id}: {fileName}): {ex.Message}");
				return Problem();
			}
		}
		[HttpDelete("delete")]
		public async Task<ActionResult> Delete(long id, string fileName)
		{
			if (!user.CanDelete)
                return Unauthorized();
            try
			{
				log.Info($"Запрос на удаление файла ({id}: {fileName})");
				var file = _context.UserFiles.Where(x => x.Name == fileName && x.OwnerId==id).FirstOrDefault();
				if (file == null)
                    return NotFound();
				System.IO.File.Delete($"UserFiles\\{id}\\{file.DiskLocation}");
				_context.Remove(file);
				_context.SaveChanges();
				log.Info($"Файл ({id}: {fileName}) успешно удален пользователем {user.Id}");
				return Ok();
			}
			catch (Exception ex)
			{
				log.Error($"Ошибка при удалении файла ({id}: {fileName}): {ex.Message}");
				return Problem();
			}
		}
	}
}