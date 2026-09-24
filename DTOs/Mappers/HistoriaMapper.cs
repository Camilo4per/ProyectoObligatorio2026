using Dominio.Entidades;
using Dominio.ValueObjects.Historia;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class HistoriaMapper
    {
        public static Historia FromDTO(HistoriaDTO dto)
        {
            // Mapeo el capítulo inicial UNA sola vez: en el modelo el inicial también
            // vive dentro de Capitulos, así que esta instancia se reutiliza abajo.
            Capitulo inicial = CapituloMapper.FromDTO(dto.CapituloInicial);

            // dto.Capitulos debe contener el CapituloInicial.
            // Si un elemento de la lista coincide con el inicial, reutilizo el
            // capítulo ya mapeado en vez de crear una copia duplicada del mismo capítulo.
            List<Capitulo> capitulos = dto.Capitulos
                .Select(c => (c == dto.CapituloInicial || (c.Id != 0 && dto.CapituloInicial != null && c.Id == dto.CapituloInicial.Id)) ? inicial : CapituloMapper.FromDTO(c))
                .ToList();

            // Garantiza que el capítulo inicial esté en la lista de capítulos:
            // si el form no lo incluyó dentro de Capitulos, lo agrego como primer capítulo.
            if (!capitulos.Contains(inicial))
                capitulos.Insert(0, inicial);

            return new Historia
            {
                Id = dto.Id,
                Titulo = new TituloHistoria(dto.Titulo),
                Sinopsis = new Sinopsis(dto.Sinopsis),
                Categorias = dto.Categorias.Select(CategoriaMapper.FromDTO).ToList(),
                CapituloInicial = inicial,
                Capitulos = capitulos,
                Estado = (EstadoHistoria) dto.Estado
            };
        }

        public static HistoriaDTO ToDTO(Historia historia)
        {
            // Mapeo el inicial una sola vez y lo reutilizo en Capitulos (espejo de FromDTO):
            // así el round-trip ToDTO -> FromDTO del Edit no duplica el capítulo inicial.
            CapituloDTO inicial = CapituloMapper.ToDTO(historia.CapituloInicial);

            return new HistoriaDTO
            {
                Id = historia.Id,
                Titulo = historia.Titulo.Valor,
                Sinopsis = historia.Sinopsis.Valor,
                // Categorías como DTOs planos (solo datos, sin referencias de dominio).
                Categorias = historia.Categorias.Select(CategoriaMapper.ToDTO).ToList(),
                CapituloInicial = inicial,
                idCapituloInicial = historia.idCapituloInicial,
                // Reutilizo la misma instancia DTO del inicial dentro de Capitulos.
                // Funciona porque EF asigna la misma instancia de entidad a ambas navegaciones
                // (relationship fixup) cuando el inicial está en la lista por Id.
                Capitulos = historia.Capitulos
                    .Select(c => (c == historia.CapituloInicial || (c.Id != 0 && historia.idCapituloInicial.HasValue && c.Id == historia.idCapituloInicial.Value)) ? inicial : CapituloMapper.ToDTO(c))
                    .ToList(),
                Estado = (int) historia.Estado
            };
        }
    }
}