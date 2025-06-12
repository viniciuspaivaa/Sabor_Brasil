namespace Sabor_Brasil.Models
{
    public class ReacaoModel
    {
        public int IdUsuario { get; set; }
        public int IdPostagem { get; set; }
        public int Tipo { get; set; } // 1 = like, -1 = deslike
    }
}