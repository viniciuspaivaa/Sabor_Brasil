using Microsoft.AspNetCore.Mvc;
using Sabor_Brasil.Models;
using Sabor_Brasil.Services;

namespace Sabor_Brasil.Controllers
{
    [ApiController]
    [Route("api/reacao")]
    public class ReacaoController : ControllerBase
    {
        private readonly ReacaoService _service;

        public ReacaoController(ReacaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Registrar([FromBody] ReacaoModel reacao)
        {
            _service.RegistrarReacao(reacao);
            return Ok();
        }

        [HttpGet("{postId}")]
        public IActionResult Contar(int postId)
        {
            var resultado = _service.ContarReacoes(postId);
            return Ok(resultado);
        }
    }
}