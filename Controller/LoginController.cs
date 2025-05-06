using Microsoft.AspNetCore.Mvc;
using Sabor_Brasil.Models;
using Sabor_Brasil.Services;

namespace Sabor_Brasil.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public LoginController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            bool sucesso = _usuarioService.VerificarLogin(login);

            if (sucesso)
                return Ok(new { mensagem = "Login realizado com sucesso!" });

            return Unauthorized(new { mensagem = "Email ou senha incorretos." });
        }
    }
}
