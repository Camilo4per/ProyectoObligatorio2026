namespace DTOs.DTOs
{
    public class LecturaDTO
    {
        public int Id { get; set; }
        public int? idUsuario { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public int? idHistoria { get; set; }
        public HistoriaDTO Historia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? idFinal { get; set; }
        public CapituloDTO? FinalAlcanzado { get; set; }
    }
}