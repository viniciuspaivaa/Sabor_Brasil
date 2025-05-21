using Microsoft.AspNetCore.Mvc;
using Sabor_Brasil.Services;
using MySql.Data.MySqlClient;
using System.IO;

namespace Sabor_Brasil.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly IConfiguration _config;

        public PostController(PostService postService, IConfiguration config)
        {
            _postService = postService;
            _config = config;
        }

        [HttpGet("{usuarioId}")]
        public IActionResult GetPosts(int usuarioId)
        {
            var posts = _postService.ObterPostsPorUsuario(usuarioId);
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult CriarPost([FromForm] int idUsuario, [FromForm] string titulo, [FromForm] string descricao, [FromForm] string cidade, [FromForm] string estado, [FromForm] IFormFile imagem)
        {
            try
            {
                string? nomeArquivo = null;
                if (imagem != null)
                {
                    var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "posts");
                    if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);
                    nomeArquivo = Guid.NewGuid() + Path.GetExtension(imagem.FileName);
                    var caminho = Path.Combine(pasta, nomeArquivo);
                    using var stream = new FileStream(caminho, FileMode.Create);
                    imagem.CopyTo(stream);
                }

                using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO postagens (idUsuarios, titulo, descricao, cidade, imagem, estado, data) VALUES (@u, @t, @d, @c, @i, @e, NOW())", conn);
                cmd.Parameters.AddWithValue("@u", idUsuario);
                cmd.Parameters.AddWithValue("@t", titulo);
                cmd.Parameters.AddWithValue("@d", descricao);
                cmd.Parameters.AddWithValue("@c", cidade);
                cmd.Parameters.AddWithValue("@i", nomeArquivo);
                cmd.Parameters.AddWithValue("@e", estado);
                cmd.ExecuteNonQuery();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO AO INSERIR POST: " + ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }
    }
}