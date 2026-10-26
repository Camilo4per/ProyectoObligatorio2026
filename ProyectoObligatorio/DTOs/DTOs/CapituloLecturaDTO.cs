namespace DTOs.DTOs
{
    public class CapituloLecturaDTO
    {
        public int Id { get; set; }

        // TipoCapitulo: 0 = Intermedio, 1 = Final.
        public int Tipo { get; set; }

        public string Titulo { get; set; }
        public string Texto { get; set; }

        // Solo Intermedio: opciones para avanzar.
        public List<OpcionLecturaDTO>? Opciones { get; set; }
    }
}
