using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesInvoice.Application.DTOs.Auth;
using SalesInvoice.Application.Services;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Infrastructure.Identity;

namespace SalesInvoice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return Unauthorized(new { success = false, message = result.Message });

            return Ok(new
            {
                success = true,
                message = result.Message,
                token = result.Token
            });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok("Logged out");
        }

        [HttpPut("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            var result = await _authService.AssignRoleAsync(dto.Email, dto.Role);
            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            var (user, roles) = await _authService.GetUserAndRolesByIdAsync(id);
            if (user == null)
                return NotFound("Usuario no encontrado");

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Cedula,
                user.UserName,
                user.LockoutEnabled,
                user.AccessFailedCount,
                concurrencyStamp = user.ConcurrencyStamp,
                role = roles.FirstOrDefault() ?? "Sin rol"
            });
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _authService.GetAllUsersWithRolesAsync();
            return Ok(users);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            var result = await _authService.UpdateUserAsync(id, dto.Email, dto.Cedula, dto.ConcurrencyStamp, dto.Role);

            return result.Success
                ? Ok(new { success = true, message = result.Message })
                : BadRequest(new { success = false, message = result.Message });
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _authService.DeleteUserAsync(id);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost("lockout/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleLockout(string id, [FromQuery] bool lockUser)
        {
            var result = await _authService.ToggleLockoutAsync(id, lockUser);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpGet("usuarios")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOnlyUsers()
        {
            var allUsers = await _authService.GetAllUsersAsync();

            var usersOnly = new List<ApplicationUser>();

            foreach (var user in allUsers)
            {
                var roles = await _authService.GetRolesAsync(user);
                if (roles.Contains("Usuario"))
                {
                    usersOnly.Add(user);
                }
            }

            // Opcional: mapear a DTO para no exponer info sensible
            var usersDto = usersOnly.Select(u => new
            {
                u.Id,
                u.Email,
                u.Cedula,
                u.UserName,
                IsBlocked = u.LockoutEnd.HasValue,
                u.AccessFailedCount
            });

            return Ok(usersDto);
        }

        [HttpPost("register-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterWithRole(RegisterWithRoleDto dto)
        {
            var result = await _authService.RegisterWithRoleAsync(dto);

            if (result.Success)
                return Ok(new { success = true, message = result.Message });

            return BadRequest(new { success = false, message = result.Message });
        }


    }
}
