namespace ProyectoSecundario.Models
{
    public class FinalesHistoriaViewModel
    {
        public int? IdHistoriaSeleccionada { get; set; }
        public List<HistoriaModel> Historias { get; set; } =
            new List<HistoriaModel>();
        public FinalesHistoriaModel? Reporte { get; set; }
    }
}
