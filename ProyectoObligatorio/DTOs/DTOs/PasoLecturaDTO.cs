namespace DTOs.DTOs
{
    public class PasoLecturaDTO
    {
        public int IdLectura { get; set; }
        public int IdHistoria { get; set; }
        public string TituloHistoria { get; set; }
        public CapituloLecturaDTO Capitulo { get; set; }

        // True cuando la lectura ya alcanzó un final: para un nuevo camino hay que
        // iniciar una lectura nueva.
        public bool LecturaFinalizada { get; set; }
    }
}
