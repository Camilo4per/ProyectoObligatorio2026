using System;

namespace DTOs.DTOs
{
    public class AuditoriaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Accion { get; set; }
        public int? IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public int? IdHistoria { get; set; }
        public string? TituloHistoria { get; set; }
    }
}
