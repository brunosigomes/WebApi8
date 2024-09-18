using WebApi8.Models;

namespace WebApi8.Dto.Livro
{
    public class LivroCriacaoDto
    {
        public string Titulo { get; set; }
        public AutorModel Autor { get; set; }
    }
}
