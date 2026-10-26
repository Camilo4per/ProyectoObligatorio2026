namespace ProyectoSecundario.Models
{
    public class HistoriaCatalogoModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Sinopsis { get; set; } = string.Empty;
        public List<CategoriaModel> Categorias { get; set; } = new List<CategoriaModel>();
    }
}
