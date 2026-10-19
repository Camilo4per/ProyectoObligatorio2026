namespace ProyectoSecundario.Models
{
    public class LecturaModel
    {
        public int Id { get; set; }
        public int? idUsuario { get; set; }
        public UsuarioModel Usuario { get; set; }
        public int? idHistoria { get; set; }
        public HistoriaModel Historia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? idFinal { get; set; }
        public CapituloModel? FinalAlcanzado { get; set; }
    }
}
