namespace DTOs.DTOs
{
    public class HistoriaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public List<CategoriaDTO> Categorias { get; set; }
        public List<int> CategoriaIds { get; set; }
        public int? idCapituloInicial { get; set; }
        public CapituloDTO CapituloInicial { get; set; }
        public List<CapituloDTO> Capitulos { get; set; }
        public int Estado { get; set; }
    }
}