using Dominio.Entidades;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class LecturaMapper
    {
        // Aca uso operadores ternarios para manejar el caso en que FinalAlcanzado sea null, evitando así posibles errores de referencia nula.
        public static Lectura FromDTO(LecturaDTO dto)
        {
            return new Lectura
            {
                Id = dto.Id,
                idUsuario = dto.idUsuario,
                Usuario = UsuarioMapper.FromDTO(dto.Usuario),
                idHistoria = dto.idHistoria,
                Historia = HistoriaMapper.FromDTO(dto.Historia),
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                FinalAlcanzado = dto.FinalAlcanzado != null
                    ? (Final) CapituloMapper.FromDTO(dto.FinalAlcanzado)
                    : null
            };
        }

        public static LecturaDTO ToDTO(Lectura lectura)
        {
            return new LecturaDTO
            {
                Id = lectura.Id,
                idUsuario = lectura.idUsuario,
                Usuario = UsuarioMapper.ToDTO(lectura.Usuario),
                idHistoria = lectura.idHistoria,
                Historia = HistoriaMapper.ToDTO(lectura.Historia),
                FechaInicio = lectura.FechaInicio,
                FechaFin = lectura.FechaFin,
                FinalAlcanzado = lectura.FinalAlcanzado != null
                    ? CapituloMapper.ToDTO(lectura.FinalAlcanzado)
                    : null
            };
        }
    }
}