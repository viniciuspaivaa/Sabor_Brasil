namespace Sabor_Brasil.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Titulo { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Cidade { get; set; } = "";
        public string Estado { get; set; } = "";
        public string ImgPrato { get; set; } = "";
        public DateTime CriadoEm { get; set; }
    }
}
