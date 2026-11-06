using Dominio.Entidades;
using Dominio.ValueObjects.Historia;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public static class HistoriaMapper
    {
        public static Historia FromDTO(HistoriaDTO dto)
        {
            // Convierte el capítulo inicial una sola vez.
            // La misma instancia se reutiliza dentro de la lista de capítulos
            // para evitar crear dos objetos representando al mismo capítulo.
            Capitulo inicial = CapituloMapper.FromDTO(dto.CapituloInicial);

            // Convierte todos los capítulos del DTO a entidades de dominio.
            // Si un capítulo corresponde al inicial, reutiliza la instancia
            // creada anteriormente en lugar de mapearlo nuevamente.
            List<Capitulo> capitulos = (dto.Capitulos ?? new List<CapituloDTO>())
                .Select(capitulo =>
                    EsCapituloInicial(capitulo, dto.CapituloInicial)
                        ? inicial
                        : CapituloMapper.FromDTO(capitulo))
                .ToList();

            // Garantiza que el capítulo inicial también esté dentro de la lista.
            if (!capitulos.Contains(inicial))
            {
                capitulos.Insert(0, inicial);
            }

            return new Historia
            {
                Id = dto.Id,
                Titulo = new TituloHistoria(dto.Titulo),
                Sinopsis = new Sinopsis(dto.Sinopsis),
                Categorias = (dto.Categorias ?? new List<CategoriaDTO>())
                    .Select(CategoriaMapper.FromDTO)
                    .ToList(),
                CapituloInicial = inicial,
                Capitulos = capitulos,
                Estado = (EstadoHistoria)dto.Estado
            };
        }

        public static HistoriaDTO ToDTO(Historia historia)
        {
            CapituloDTO inicial =
                CapituloMapper.ToDTO(historia.CapituloInicial);

            return new HistoriaDTO
            {
                Id = historia.Id,
                Titulo = historia.Titulo.Valor,
                Sinopsis = historia.Sinopsis.Valor,
                Categorias = historia.Categorias
                    .Select(c => CategoriaMapper.ToDTO(c))
                    .ToList(),
                CapituloInicial = inicial,
                idCapituloInicial = historia.idCapituloInicial,
                Capitulos = historia.Capitulos
                    .Select(capitulo => EsCapituloInicial(capitulo, historia) ? inicial : CapituloMapper.ToDTO(capitulo))
                    .ToList(),
                Estado = (int)historia.Estado
            };
        }

        public static HistoriaCatalogoDTO ToCatalogoDTO(Historia historia)
        {
            return new HistoriaCatalogoDTO
            {
                Id = historia.Id,
                Titulo = historia.Titulo.Valor,
                Sinopsis = historia.Sinopsis.Valor,
                Categorias = historia.Categorias
                    .Select(c => CategoriaMapper.ToDTO(c))
                    .ToList()
            };
        }

        // Para el FromDTO
        private static bool EsCapituloInicial(
            CapituloDTO capitulo,
            CapituloDTO inicial)
        {
            return capitulo == inicial ||
                   (capitulo.Id != 0 &&
                    inicial != null &&
                    capitulo.Id == inicial.Id);
        }
        // Para el ToDTO
        private static bool EsCapituloInicial(
            Capitulo capitulo,
            Historia historia)
        {
            return capitulo == historia.CapituloInicial ||
                   (capitulo.Id != 0 &&
                    historia.idCapituloInicial.HasValue &&
                    capitulo.Id == historia.idCapituloInicial.Value);
        }
    }
}