namespace ProyectoSecundario.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Nombre { get; init; }
        public string Apellido { get; init; }
        public string NombreUsuario { get; set; }
        public int Rol { get; set; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
