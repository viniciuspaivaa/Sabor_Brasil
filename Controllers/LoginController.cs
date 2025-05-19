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
            var usuario = _usuarioService.ValidarLogin(request.Email, request.Senha);

            if (usuario == null)
                return Unauthorized(new { mensagem = "Email ou senha incorretos." });

            return Ok(new { 
                id = usuario.Id,
                nome = usuario.Nome, 
                imagem = usuario.Imagem 
            });
        }
    }
}
