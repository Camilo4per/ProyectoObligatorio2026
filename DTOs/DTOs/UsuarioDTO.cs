using Dominio.Entidades;

namespace DTOs.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public RolesUsuario Rol { get; set; }

        // Value Object Email
        public string Email { get; init; }

        // Value Object Password
        public string Password { get; init; }
    }
}
