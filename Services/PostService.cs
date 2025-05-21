using MySql.Data.MySqlClient;
using Sabor_Brasil.Models;

namespace Sabor_Brasil.Services
{
    public class PostService
    {
        private readonly string _connectionString;

        public PostService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MySqlConnection")!;
        }

        public List<Post> ObterPostsPorUsuario(int usuarioId)
        {
            var posts = new List<Post>();

            using var conexao = new MySqlConnection(_connectionString);
            conexao.Open();

            var comando = new MySqlCommand("SELECT * FROM postagens WHERE idUsuarios = @usuarioId ORDER BY data DESC", conexao);
            comando.Parameters.AddWithValue("@usuarioId", usuarioId);

            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                posts.Add(new Post
                {
                    Id = reader.GetInt32("id"),
                    Titulo = reader.GetString("titulo"),
                    Descricao = reader.GetString("descricao"),
                    UsuarioId = reader.GetInt32("idUsuarios"),
                    Cidade = reader.GetString("cidade"),
                    Estado = reader.GetString("estado"),
                    ImgPrato = reader.GetString("imagem"),
                    CriadoEm = reader.GetDateTime("data")
                });
            }

            return posts;
        }

        public List<Post> ObterTodosPosts()
        {
            var posts = new List<Post>();
            using var conexao = new MySqlConnection(_connectionString);
            conexao.Open();

            var comando = new MySqlCommand("SELECT * FROM postagens ORDER BY data DESC", conexao);

            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                posts.Add(new Post
                {
                    Id = reader.GetInt32("id"),
                    Titulo = reader.GetString("titulo"),
                    Descricao = reader.GetString("descricao"),
                    UsuarioId = reader.GetInt32("idUsuarios"),
                    Cidade = reader.GetString("cidade"),
                    Estado = reader.GetString("estado"),
                    ImgPrato = reader.GetString("imagem"),
                    CriadoEm = reader.GetDateTime("data")
                });
            }

            return posts;
        }
    }
}
