using Dominio.Entidades;

namespace DTOs.DTOs
{
    public class HistoriaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public List<Categoria> Categorias { get; set; }
        public Capitulo Capitulo { get; set; }
        public EstadoHistoria Estado { get; set; }
    }
}