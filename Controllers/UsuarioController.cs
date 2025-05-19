using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

[ApiController]
[Route("api/usuario")]
public class UsuarioController : ControllerBase
{
    private readonly IConfiguration _config;
    public UsuarioController(IConfiguration config) => _config = config;

    [HttpPost]
    public IActionResult Cadastrar([FromForm] string nome, [FromForm] string email, [FromForm] string senha, [FromForm] IFormFile? imagem)
    {
        string? nomeArquivo = null;
        if (imagem != null)
        {
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "usuarios");
            if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);
            nomeArquivo = Guid.NewGuid() + Path.GetExtension(imagem.FileName);
            var caminho = Path.Combine(pasta, nomeArquivo);
            using var stream = new FileStream(caminho, FileMode.Create);
            imagem.CopyTo(stream);
        }

        using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
        conn.Open();
        var cmd = new MySqlCommand("INSERT INTO usuarios (nome, email, senha, imagem) VALUES (@n, @e, @s, @i)", conn);
        cmd.Parameters.AddWithValue("@n", nome);
        cmd.Parameters.AddWithValue("@e", email);
        cmd.Parameters.AddWithValue("@s", senha);
        cmd.Parameters.AddWithValue("@i", nomeArquivo);
        try
        {
            cmd.ExecuteNonQuery();
            return Ok();
        }
        catch (MySqlException ex)
        {
            if (ex.Number == 1062) // Duplicate entry
                return BadRequest(new { mensagem = "Email já cadastrado!" });
            throw;
        }
    }
}