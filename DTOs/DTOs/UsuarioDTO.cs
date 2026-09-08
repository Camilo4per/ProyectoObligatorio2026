namespace DTOs.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public int Rol { get; set; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
