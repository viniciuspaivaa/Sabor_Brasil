using MySql.Data.MySqlClient;
using Sabor_Brasil.Models;

namespace Sabor_Brasil.Services
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MySqlConnection")!;
        }

        public Usuario? ValidarLogin(string email, string senha)
        {
            using var conexao = new MySqlConnection(_connectionString);
            conexao.Open();

            var comando = new MySqlCommand("SELECT * FROM usuarios WHERE email = @email AND senha = @senha", conexao);
            comando.Parameters.AddWithValue("@email", email);
            comando.Parameters.AddWithValue("@senha", senha);

            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Usuario
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Email = reader.GetString("email"),
                    Senha = reader.GetString("senha"),
                    Imagem = reader.GetString("imagem")
                };
            }

            return null;
        }
    }
}
