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
        public IActionResult Login([FromBody] LoginRequest request)
        {
            bool sucesso = _usuarioService.VerificarLogin(login);

            if (sucesso) {
                return Ok(new { mensagem = "Login realizado com sucesso!" });
                return Ok(new { nome = usuario.Nome }); // Retorna o nome
            } else {
                return Unauthorized(new { mensagem = "Email ou senha incorretos." });
            }
        }
    }
}
