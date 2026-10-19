namespace ProyectoSecundario.Models
{
    public class HistoriaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public List<CategoriaModel> Categorias { get; set; }
        public List<int> CategoriaIds { get; set; }
        public int? idCapituloInicial { get; set; }
        public CapituloModel CapituloInicial { get; set; }
        public List<CapituloModel> Capitulos { get; set; }
        public int Estado { get; set; }
    }
}