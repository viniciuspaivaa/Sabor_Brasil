using MySql.Data.MySqlClient;
using Sabor_Brasil.Models;

namespace Sabor_Brasil.Services
{
    public class ReacaoService
    {
        private readonly string _connectionString;

        public ReacaoService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MySqlConnection")!;
        }

        public void RegistrarReacao(ReacaoModel reacao)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            // Verifica se já existe
            var checkCmd = new MySqlCommand("SELECT id FROM likesDeslikes WHERE idUsuario = @usuario AND idPostagem = @post", conn);
            checkCmd.Parameters.AddWithValue("@usuario", reacao.IdUsuario);
            checkCmd.Parameters.AddWithValue("@post", reacao.IdPostagem);

            var existe = checkCmd.ExecuteScalar();

            if (existe != null)
            {
                // Atualiza
                var update = new MySqlCommand("UPDATE likesDeslikes SET tipo = @tipo WHERE idUsuario = @usuario AND idPostagem = @post", conn);
                update.Parameters.AddWithValue("@tipo", reacao.Tipo);
                update.Parameters.AddWithValue("@usuario", reacao.IdUsuario);
                update.Parameters.AddWithValue("@post", reacao.IdPostagem);
                update.ExecuteNonQuery();
            }
            else
            {
                // Insere
                var insert = new MySqlCommand("INSERT INTO likesDeslikes (idUsuario, idPostagem, tipo) VALUES (@usuario, @post, @tipo)", conn);
                insert.Parameters.AddWithValue("@usuario", reacao.IdUsuario);
                insert.Parameters.AddWithValue("@post", reacao.IdPostagem);
                insert.Parameters.AddWithValue("@tipo", reacao.Tipo);
                insert.ExecuteNonQuery();
            }
        }

        public object ContarReacoes(int postId)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"
            SELECT 
                SUM(CASE WHEN tipo = 1 THEN 1 ELSE 0 END) AS Likes,
                SUM(CASE WHEN tipo = -1 THEN 1 ELSE 0 END) AS Deslikes
            FROM likesDeslikes
            WHERE idPostagem = @postId", conn);
            cmd.Parameters.AddWithValue("@postId", postId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new
                {
                    likes = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    deslikes = reader.IsDBNull(1) ? 0 : reader.GetInt32(1)
                };
            }

            return new { likes = 0, deslikes = 0 };
        }
    }
}