namespace DTOs.DTOs
{
    public class OpcionDTO
    {
        public string Texto { get; set; }
        public int? idCapitulo { get; set; }
        public int? idCapDestino { get; set; }
        public CapituloDTO? CapDestino { get; set; }
    }
}