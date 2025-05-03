using Login.API.Data;
using Login.API.Models;
using Login.API.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Login.API.Request;
using Microsoft.EntityFrameworkCore;

namespace Login.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginContext _context;
        private readonly UserService _userService;

        public AuthController(LoginContext context)
        {
            _context = context;
            _userService = new UserService();
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Request.RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o Email já existe
            if (await _context.TblUser.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email já está em uso.");

            // Cria um novo UserClass com os dados do UserRequest
            var user = new UserClass
            {
                Email = request.Email,
                Username = request.Username,
                PasswordHash = _userService.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = false,
                EmailConfirmationToken = Guid.NewGuid().ToString(),
                EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24),
                PasswordResetToken = Guid.NewGuid().ToString(),
                PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            _context.TblUser.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Usuário registrado com sucesso!");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Request.LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.TblUser.FirstOrDefaultAsync(u => u.Email == request.Identifier ||u.Username == request.Identifier);

            if (user == null)
                return Unauthorized("Usuário não encontrado.");

            if (!_userService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Senha inválida.");

            return Ok("Login realizado com sucesso!");
        }
    }
}
