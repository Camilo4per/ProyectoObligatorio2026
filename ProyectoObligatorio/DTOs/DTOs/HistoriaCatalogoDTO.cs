namespace DTOs.DTOs
{
    // Datos mínimos que necesita el catálogo del lector: no expone capítulos ni
    // campos de administración (CantLlegaron, idCapituloAntecedente, etc.).
    public class HistoriaCatalogoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public List<CategoriaDTO> Categorias { get; set; }
    }
}
