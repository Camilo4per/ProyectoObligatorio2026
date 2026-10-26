namespace ProyectoSecundario.Models
{
    public class CatalogoViewModel
    {
        public List<HistoriaCatalogoModel> Historias { get; set; } = new List<HistoriaCatalogoModel>();
        public List<CategoriaModel> Categorias { get; set; } = new List<CategoriaModel>();
        public int? IdCategoriaSeleccionada { get; set; }
    }
}
