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
                Usuario = UsuarioMapper.FromDTO(dto.Usuario),
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
                Usuario = UsuarioMapper.ToDTO(lectura.Usuario),
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