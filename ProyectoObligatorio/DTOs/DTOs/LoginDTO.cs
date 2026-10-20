using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
