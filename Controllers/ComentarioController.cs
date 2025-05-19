using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

[ApiController]
[Route("api/comentario")]
public class ComentarioController : ControllerBase
{
    private readonly IConfiguration _config;
    public ComentarioController(IConfiguration config) => _config = config;

    [HttpGet("{postId}")]
    public IActionResult GetComentarios(int postId)
    {
        var lista = new List<object>();
        using var conn = new MySqlConnection(_config.GetConnectionString("MySqlConnection"));
        conn.Open();
        var cmd = new MySqlCommand(@"
            SELECT c.texto, c.data, c.imagem, u.nome as nomeUsuario
            FROM comentarios c
            LEFT JOIN usuarios u ON u.id = c.idUsuario
            WHERE c.idPostagem = @postId
            ORDER BY c.data DESC", conn);
        cmd.Parameters.AddWithValue("@postId", postId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new {
                texto = reader.GetString(0),
                data = reader.GetDateTime(1),
                imagem = reader.IsDBNull(2) ? null : reader.GetString(2),
                nomeUsuario = reader.IsDBNull(3) ? null : reader.GetString(3)
            });
        }
        return Ok(lista);
    }
}