using Dominio.Entidades;
using Dominio.Excepciones;
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
                idFinal = dto.idFinal,
                FinalAlcanzado = MapearFinalAlcanzado(dto.FinalAlcanzado)
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
                idFinal = lectura.idFinal,
                FinalAlcanzado = lectura.FinalAlcanzado != null
                    ? CapituloMapper.ToDTO(lectura.FinalAlcanzado)
                    : null
            };
        }

        // El final alcanzado en una lectura es siempre un Final: si el DTO indica otra
        // cosa se avisa con la excepción de contexto, en vez de castear a ciegas.
        private static Final? MapearFinalAlcanzado(CapituloDTO? dto)
        {
            if (dto == null) return null;

            Capitulo capitulo = CapituloMapper.FromDTO(dto);
            if (capitulo is Final final) return final;

            throw new LecturaException("El capítulo alcanzado en una lectura debe ser de tipo final");
        }
    }
}
