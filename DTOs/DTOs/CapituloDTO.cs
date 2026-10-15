
namespace DTOs.DTOs
{
    public class CapituloDTO
    {
        public int Id { get; set; }

        // TipoCapitulo: 0 = Intermedio, 1 = Final. Es el dato que define el tipo del
        // capítulo (la colección Opciones ya no lo representa).
        public int Tipo { get; set; }

        public string Titulo { get; set; }
        public string Texto { get; set; }
        public int? idHistoria { get; set; }

        // Solo Final
        public int CantLlegaron { get; set; }

        // Solo Intermedio
        public List<OpcionDTO>? Opciones { get; set; }
    }
}
