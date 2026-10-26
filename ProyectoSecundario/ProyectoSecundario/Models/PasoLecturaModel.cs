namespace ProyectoSecundario.Models
{
    public class PasoLecturaModel
    {
        public int IdLectura { get; set; }
        public int IdHistoria { get; set; }
        public string TituloHistoria { get; set; } = string.Empty;
        public CapituloLecturaModel Capitulo { get; set; } = new CapituloLecturaModel();
        public bool LecturaFinalizada { get; set; }
    }
}
