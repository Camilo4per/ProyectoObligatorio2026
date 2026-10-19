namespace ProyectoSecundario.Models
{
    public class TokenModel
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioModel Usuario { get; set; } = new UsuarioModel();
    }
}
