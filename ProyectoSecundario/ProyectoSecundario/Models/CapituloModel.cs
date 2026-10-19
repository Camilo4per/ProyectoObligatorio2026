
namespace ProyectoSecundario.Models
{
    public class CapituloModel
    {
        public int Id { get; set; }

        // TipoCapitulo: 0 = Intermedio, 1 = Final.
        public int Tipo { get; set; } // Con esto puedo saber si el usuario llegó a un capítulo final o no. Si es final, no hay opciones para elegir.
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public int? idHistoria { get; set; }
        public int CantLlegaron { get; set; }
        public List<OpcionModel>? Opciones { get; set; }

        // Datos para encadenamiento al crear el capítulo
        public int? idCapituloAntecedente { get; set; }
        public string? TextoOpcion { get; set; }
    }
}
