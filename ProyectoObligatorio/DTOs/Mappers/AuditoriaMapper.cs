using Dominio.Entidades;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class AuditoriaMapper
    {
        public static AuditoriaDTO ToDTO(Auditoria auditoria)
        {
            if (auditoria == null) return null!;

            return new AuditoriaDTO
            {
                Id = auditoria.Id,
                Fecha = auditoria.Fecha,
                Accion = auditoria.Accion,
                IdUsuario = auditoria.idUsuario,
                NombreUsuario = auditoria.Usuario?.NombreUsuario,
                IdHistoria = auditoria.idHistoria,
                TituloHistoria = auditoria.Historia?.Titulo?.Valor
            };
        }

        public static Auditoria FromDTO(AuditoriaDTO dto)
        {
            if (dto == null) return null!;

            return new Auditoria
            {
                Id = dto.Id,
                Fecha = dto.Fecha,
                Accion = dto.Accion,
                idUsuario = dto.IdUsuario,
                idHistoria = dto.IdHistoria
            };
        }
    }
}
