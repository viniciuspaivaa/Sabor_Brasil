using MySql.Data.MySqlClient;
using Sabor_Brasil.Models;

namespace Sabor_Brasil.Services
{
    public class UsuarioService
    {
        private readonly string? _connectionString;

        public UsuarioService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MySqlConnection");
        }

        public bool VerificarLogin(LoginRequest login)
        {
            using var conexao = new MySqlConnection(_connectionString);
            conexao.Open();

            var comando = new MySqlCommand("SELECT COUNT(*) FROM usuarios WHERE email = @email AND senha = @senha", conexao);
            comando.Parameters.AddWithValue("@email", login.Email);
            comando.Parameters.AddWithValue("@senha", login.Senha); // Em produção, use senha criptografada!

            var resultado = Convert.ToInt32(comando.ExecuteScalar());
            return resultado > 0;
        }
    }
}
