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
                Titulo = new TituloHistoria(dto.Titulo),
                Sinopsis = new Sinopsis(dto.Sinopsis),
                Categorias = dto.Categorias.Select(CategoriaMapper.FromDTO).ToList(),
                CapituloInicial = CapituloMapper.FromDTO(dto.CapituloInicial),
                Capitulos = dto.Capitulos.Select(CapituloMapper.FromDTO).ToList(),
                Estado = (EstadoHistoria) dto.Estado
            };
        }

        public static HistoriaDTO ToDTO(Historia historia)
        {
            return new HistoriaDTO
            {
                Id = historia.Id,
                Titulo = historia.Titulo.Valor,
                Sinopsis = historia.Sinopsis.Valor,
                Categorias = historia.Categorias.Select(CategoriaMapper.ToDTO).ToList(),
                CapituloInicial = CapituloMapper.ToDTO(historia.CapituloInicial),
                Capitulos = historia.Capitulos.Select(CapituloMapper.ToDTO).ToList(),
                Estado = (int) historia.Estado
            };
        }
    }
}