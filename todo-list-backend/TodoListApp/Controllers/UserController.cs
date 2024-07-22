using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoListApp.Data;
using TodoListApp.DTOs;
using TodoListApp.Models;
using TodoListApp.Services;
using Serilog;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
namespace TodoListApp.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, ILogger<UserController> logger, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;

        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserDTO dto)
        {
            _logger.LogInformation("Login method called.");
            var user = _context.Users.SingleOrDefault(u => u.Username == dto.Username && u.Password == dto.Password);

            if (user != null)
            {
                var token = _tokenService.CreateToken(user);
                _logger.LogInformation("User logged in successfully: {Username}", dto.Username);
                return Ok(new
                {
                    Token = token,
                    UserId = user.Id
                });

            }


            return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

        }
        //[Authorize]
        [HttpPost]
        [Route("adduser")]
        public IActionResult AddUser(UserDTO dto)
        {
            _logger.LogInformation("AddUser method called.");

            User user = new User
            {
                Username = dto.Username,
                Password = dto.Password
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            _logger.LogInformation("User successfully added: {Username}", dto.Username);
            return Ok("Kullanıcı başarıyla eklendi!");
        }

       [HttpGet]
        public IActionResult AddUser()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _logger.LogInformation("DeleteUser method called for User ID: {UserId}", id);
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);

            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                return NotFound("Belirtilen Id'ye sahip kullanıcı bulunamadı.");
            }

            _context.Users.Remove(existingUser);
            _context.SaveChanges();

            _logger.LogInformation("User deleted successfully for User ID: {UserId}", id);
            return Ok("Kullanıcı başarıyla silindi!");
        }

    }
}




        //[Authorize]
        /*  [HttpGet("{id}")]
          public IActionResult GetUser(int id)
          {

              _logger.LogInformation("GetUser method called for User ID: {UserId}", id);
              var user = _context.Users.FirstOrDefault(u => u.Id == id);
              if (user == null)
              {
                  _logger.LogWarning("User with ID {UserId} not found.", id);
                  return NotFound();
              }

              _logger.LogInformation("User retrieved successfully for User ID: {UserId}", id);
              return Ok(user);
          }
          //[Authorize]
          [HttpPut("{id}")]

          public IActionResult UpdateUser(int id, [FromBody] UserDTO dto)
          {
              _logger.LogInformation("UpdateUser method called for User ID: {UserId}", id);
              var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);

              if (existingUser == null)
              {
                  _logger.LogWarning("User with ID {UserId} not found.", id);
                  return NotFound("Belirtilen Id'ye sahip kullanıcı bulunamadı.");
              }

              existingUser.Username = dto.Username;
              existingUser.Password = dto.Password;

              _context.SaveChanges();
              _logger.LogInformation("User updated successfully for User ID: {UserId}", id);
              return Ok("Kullanıcı başarıyla güncellendi!");
          }
         // [Authorize]
          [HttpDelete("{id}")]
          public IActionResult DeleteUser(int id)
          {
              _logger.LogInformation("DeleteUser method called for User ID: {UserId}", id);
              var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);

              if (existingUser == null)
              {
                  _logger.LogWarning("User with ID {UserId} not found.", id);
                  return NotFound("Belirtilen Id'ye sahip kullanıcı bulunamadı.");
              }

              _context.Users.Remove(existingUser);
              _context.SaveChanges();

              _logger.LogInformation("User deleted successfully for User ID: {UserId}", id);
              return Ok("Kullanıcı başarıyla silindi!");
          }

    }
}*/