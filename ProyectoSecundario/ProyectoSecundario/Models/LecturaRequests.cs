namespace ProyectoSecundario.Models
{
    // Request que viaja a POST /api/Lectura.
    public class IniciarLecturaModel
    {
        public int idHistoria { get; set; }
        public int idUsuario { get; set; }
    }

    // Request que viaja a POST /api/Lectura/{id}/Avanzar.
    public class AvanzarLecturaModel
    {
        public int idUsuario { get; set; }
        public int idCapituloOrigen { get; set; }
        public int idCapDestino { get; set; }
    }
}
