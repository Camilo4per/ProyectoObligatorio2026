using Dominio.Entidades;
using Dominio.ValueObjects.Historia;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class HistoriaMapper
    {
        public static Historia FromDTO(HistoriaDTO dto)
        {
            return new Historia
            {
                Id = dto.Id,
                Titulo = new Titulo(dto.Titulo),
                Sinopsis = new Sinopsis(dto.Sinopsis),
                Categorias = dto.Categorias.Select(CategoriaMapper.FromDTO).ToList(),
                Capitulo = CapituloMapper.FromIntermedioDTO(dto.Capitulo),
                Estado = (EstadoHistoria) dto.Estado
            };
        }
    }
}