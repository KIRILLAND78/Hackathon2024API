using Hackathon2024API.Data;
using Hackathon2024API.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hackathon2024API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class FileExtentionController : Controller
    {
        ApplicationDbContext _context;
        public FileExtentionController(ApplicationDbContext db) { _context = db; }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_context.FileExtentions.ToList());
        }
        [HttpGet("User")]
        public IActionResult User([FromQuery] long id)
        {
            var user = _context.Users.Where(x => x.Id == id).Include(x=>x.FileExtention).FirstOrDefault();
            if (user == null) return NotFound();
            return Ok(user.FileExtention.ToList());
        }
        [HttpPost("User")]
        public IActionResult UserPost([FromQuery] long id, [FromQuery] long extentionId)
        {
            var user = _context.Users.Where(x => x.Id == id).Include(x => x.FileExtention).FirstOrDefault();
            if (user == null) return NotFound("User not found");
            var extention = _context.FileExtentions.Where(x => x.Id == extentionId).FirstOrDefault();
            if (extention == null) return NotFound("Extention not found");
            user.FileExtention.Add(extention);
            _context.SaveChanges();
            return Ok(user.FileExtention.ToList());
        }
        [HttpDelete("User")]
        public IActionResult UserDelete([FromQuery] long id, [FromQuery] long extentionId)
        {
            var user = _context.Users.Where(x => x.Id == id).Include(x => x.FileExtention).FirstOrDefault();
            if (user == null) return NotFound("User not found");
            var extention = _context.FileExtentions.Where(x => x.Id == extentionId).FirstOrDefault();
            if (extention == null) return NotFound("Extention not found");
            user.FileExtention.Remove(extention);
            _context.SaveChanges();
            return Ok(user.FileExtention.ToList());
        }
        [HttpPost]
        public IActionResult Create([FromQuery] string title)
        {
            FileExtention extention = new FileExtention() { Title = title };
            _context.FileExtentions.Add(extention);
            _context.SaveChanges();
            return Ok(extention);
        }
        [HttpDelete]
        public IActionResult Delete([FromQuery] long id)
        {
            FileExtention extention = _context.FileExtentions.Where(x => x.Id == id).FirstOrDefault();
            if (extention == null) return NotFound();
            _context.FileExtentions.Remove(extention);
            _context.SaveChanges();
            return Ok(extention);
        }
        [HttpPatch]
        public IActionResult Patch([FromQuery] long id, [FromQuery] string title)
        {
            FileExtention extention = _context.FileExtentions.Where(x => x.Id == id).FirstOrDefault();
            if (extention == null) return NotFound();
            extention.Title = title;
            _context.SaveChanges();
            return Ok(extention);
        }
    }
}
