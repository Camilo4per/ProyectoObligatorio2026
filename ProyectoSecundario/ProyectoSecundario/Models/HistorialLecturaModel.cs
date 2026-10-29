namespace ProyectoSecundario.Models
{
    public class HistorialLecturaModel
    {
        public int IdLectura { get; set; }
        public string Historia { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Completada { get; set; }
        public string? FinalAlcanzado { get; set; }
    }
}
