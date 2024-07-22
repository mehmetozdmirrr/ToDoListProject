using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.DTOs;
using TodoListApp.Models;
using Serilog;
using TodoListApp.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TodoListApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NoteController> _logger;

        public NoteController(ApplicationDbContext context, ILogger<NoteController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //[AllowAnonymous]
        [HttpPost]
        public IActionResult AddNote([FromBody] NoteDTO dto)
        {
            //var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //{
            //    _logger.LogError("UserId claim is missing.");
            //    return Unauthorized("UserId claim is missing.");
            //}

            var userId = dto.UserId;//int.Parse(userIdClaim.Value);

            _logger.LogInformation("AddNote method called.");
            Note note = new Note
            {
                Text = dto.Text,
                Title = dto.Title,
                userId = userId
            };

            _context.Notes.Add(note);
            _context.SaveChanges();

            _logger.LogInformation("Note successfully added.");
            return Ok("Not başarıyla eklendi!");
        }
        [Authorize]
        [HttpGet]
        //[Route("getId")]
        public IActionResult GetNote(int UserId)
        {
            /*var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogError("UserId claim is missing.");
                return Unauthorized("UserId claim is missing.");
            }
            */
            //var userId = dto.UserId;

            _logger.LogInformation("GetNote method called with id: {Id}", UserId);
            var note = _context.Notes.Where(n => n.userId == UserId && !n.IsDeleted).ToList();
            if (note == null)
            {
                _logger.LogWarning("Note not found with id: {Id}", UserId);
                return NotFound();
            }

            return Ok(note);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, [FromBody] NoteDTO dto)
        {
            /* var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
             if (userIdClaim == null)
             {
                 _logger.LogError("UserId claim is missing.");
                 return Unauthorized("UserId claim is missing.");
             }*/

            var userId = dto.UserId;

            _logger.LogInformation("UpdateNote method called with id: {Id}", id);
            var existingNote = _context.Notes.FirstOrDefault(n => n.Id == id && n.userId == userId);

            if (existingNote == null)
            {
                _logger.LogWarning("Note not found with id: {Id}", id);
                return NotFound("Belirtilen Id'ye sahip not bulunamadı.");
            }

            existingNote.Text = dto.Text;
            existingNote.Title = dto.Title;

            _context.SaveChanges();

            _logger.LogInformation("Note successfully updated with id: {Id}", id);
            return Ok("Not başarıyla güncellendi!");
        }
      
        [HttpDelete]
        public IActionResult DeleteNote(int id)
        {
            /* var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
             if (userIdClaim == null)
             {
                 _logger.LogError("UserId claim is missing.");
                 return Unauthorized("UserId claim is missing.");
             }
            */
            //var userId = dto.UserId;

            _logger.LogInformation("DeleteNote method called with id: {Id}", id);
            var existingNote = _context.Notes.FirstOrDefault(n => n.Id == id);
            //var note = _context.Notes.FirstOrDefault(p=> p.Id=id);

            /*if (existingNote == null)
            {
                _logger.LogWarning("Note not found with id: {Id}", id);
                return NotFound("Belirtilen Id'ye sahip not bulunamadı.");
            }
             existingNote.IsDeleted = true;
            //_context.Notes.Remove(existingNote);*/
            existingNote.IsDeleted = true;
            _context.SaveChanges();

            _logger.LogInformation("Note successfully deleted with id: {Id}", id);
            return Ok("Not başarıyla silindi!");
        }
    }
}
