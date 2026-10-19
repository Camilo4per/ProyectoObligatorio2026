using System.ComponentModel.DataAnnotations;

namespace ProyectoSecundario.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
