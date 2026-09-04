
namespace DTOs.DTOs
{
    public class IntermedioDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public List<OpcionDTO> Opciones { get; set; }
    }
}