namespace ProyectoSecundario.Models
{
    public class CapituloLecturaModel
    {
        public int Id { get; set; }

        // TipoCapitulo: 0 = Intermedio, 1 = Final.
        public int Tipo { get; set; }

        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public List<OpcionLecturaModel>? Opciones { get; set; }
    }
}
