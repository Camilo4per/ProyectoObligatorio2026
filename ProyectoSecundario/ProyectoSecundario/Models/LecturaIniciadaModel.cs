namespace ProyectoSecundario.Models
{
    public class LecturaIniciadaModel
    {
        public int IdLectura { get; set; }
        public int IdHistoria { get; set; }
        public string TituloHistoria { get; set; } = string.Empty;
        public int IdCapituloInicial { get; set; }
    }
}
