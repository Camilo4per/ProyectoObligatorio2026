using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class ModificarHistoriaCU : IModificarHistoria
    {
        private IRepositorioHistorias repositorioHistorias;
        private IRepositorioCategorias repositorioCategorias;
        public ModificarHistoriaCU(IRepositorioHistorias repoHistorias, IRepositorioCategorias repoCategorias)
        {
            repositorioHistorias = repoHistorias;
            repositorioCategorias = repoCategorias;
        }

        public void Ejecutar(HistoriaDTO dto)
        {
            HistoriaDTO completo = HistoriaMapper.ToDTO(repositorioHistorias.FindById(dto.Id));

            if (dto.CapituloInicial != null && completo.CapituloInicial != null)
            {
                bool yaTieneOtrosCapitulos = completo.Capitulos != null && completo.Capitulos.Count > 1;
                bool cambioTitulo = !string.Equals(dto.CapituloInicial.Titulo, completo.CapituloInicial.Titulo);
                bool cambioTexto = !string.Equals(dto.CapituloInicial.Texto, completo.CapituloInicial.Texto);

                if (yaTieneOtrosCapitulos && (cambioTitulo || cambioTexto))
                {
                    throw new HistoriaException(
                        "El capítulo inicial no se puede modificar porque la historia ya posee otros capítulos agregados.");
                }

                if (!yaTieneOtrosCapitulos)
                {
                    completo.CapituloInicial.Titulo = dto.CapituloInicial.Titulo;
                    completo.CapituloInicial.Texto = dto.CapituloInicial.Texto;
                    if (completo.Capitulos != null && completo.Capitulos.Count > 0)
                    {
                        completo.Capitulos[0].Titulo = dto.CapituloInicial.Titulo;
                        completo.Capitulos[0].Texto = dto.CapituloInicial.Texto;
                    }
                }
            }

            completo.Titulo = dto.Titulo;
            completo.Sinopsis = dto.Sinopsis;
            completo.Estado = dto.Estado;
            completo.CategoriaIds = dto.CategoriaIds;
            completo.Categorias = ResolverCategorias(dto.CategoriaIds);

            repositorioHistorias.Update(HistoriaMapper.FromDTO(completo));
        }

        private List<CategoriaDTO> ResolverCategorias(List<int> ids)
        {
            List<CategoriaDTO> resultado = new List<CategoriaDTO>();
            if (ids != null)
            {
                foreach (int id in ids)
                {
                    resultado.Add(CategoriaMapper.ToDTO(repositorioCategorias.FindById(id)));
                }
            }
            return resultado;
        }
    }
}