namespace DTOs.DTOs
{
    public class FinalesHistoriaDTO
    {
        public int IdHistoria { get; set; }
        public string TituloHistoria { get; set; } = string.Empty;
        public List<FinalFrecuenciaDTO> Finales { get; set; } =
            new List<FinalFrecuenciaDTO>();
    }
}
