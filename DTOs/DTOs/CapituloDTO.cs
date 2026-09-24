
namespace DTOs.DTOs
{
    public class CapituloDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public int? idHistoria { get; set; }

        // FinalDTO
        public int CantLlegaron { get; set; }

        // IntermedioDTO
        public List<OpcionDTO>? Opciones { get; set; }
    }
}