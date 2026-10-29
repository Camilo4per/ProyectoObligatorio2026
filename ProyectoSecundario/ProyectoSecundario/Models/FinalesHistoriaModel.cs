namespace ProyectoSecundario.Models
{
    public class FinalesHistoriaModel
    {
        public int IdHistoria { get; set; }
        public string TituloHistoria { get; set; } = string.Empty;
        public List<FinalFrecuenciaModel> Finales { get; set; } =
            new List<FinalFrecuenciaModel>();
    }
}
