namespace ProyectoSecundario.Models
{
    public class FinalFrecuenciaModel
    {
        public int IdFinal { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int CantidadVecesAlcanzado { get; set; }
        public bool EsMasFrecuente { get; set; }
    }
}
