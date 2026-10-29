namespace ProyectoSecundario.Models
{
    public class UsuarioRankingModel
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int CantidadHistoriasCompletadas { get; set; }
    }
}
