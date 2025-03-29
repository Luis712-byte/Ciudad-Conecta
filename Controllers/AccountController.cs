using Microsoft.AspNetCore.Mvc;
using ProyectoReportes.Models.DTOs;
using System.Threading.Tasks;

namespace ProyectoReportes.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);
            return result == "Cuenta registrada correctamente" ? Ok(new { message = result }) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(request);
            return token == null ? Unauthorized("Credenciales incorrectas.") : Ok(new { token });
        }
    }
}
