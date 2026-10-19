namespace ProyectoSecundario.Models
{
    public class OpcionModel
    {
        public string Texto { get; set; }
        public int? idCapitulo { get; set; }
        public int? idCapDestino { get; set; }
        public CapituloModel? CapDestino { get; set; }
    }
}