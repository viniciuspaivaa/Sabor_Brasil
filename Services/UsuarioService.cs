using MySql.Data.MySqlClient;

namespace Sabor_Brasil.Services
{
    public class UsuarioService
    {
        private readonly string? _connectionString;

        public UsuarioService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MySqlConnection");
        }

        public void TestarConexao()
        {
            using var conexao = new MySqlConnection(_connectionString);
            conexao.Open();
            Console.WriteLine("Conectado com sucesso ao MySQL!");
        }
    }
}
