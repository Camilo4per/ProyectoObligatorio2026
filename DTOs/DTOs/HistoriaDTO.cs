using Dominio.Entidades;

namespace DTOs.DTOs
{
    public class HistoriaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public List<CategoriaDTO> Categorias { get; set; }
        public CapituloDTO Capitulo { get; set; }
        public int Estado { get; set; }
    }
}