namespace DTOs.DTOs
{
    public class FinalFrecuenciaDTO
    {
        public int IdFinal { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int CantidadVecesAlcanzado { get; set; }
        public bool EsMasFrecuente { get; set; }
    }
}
