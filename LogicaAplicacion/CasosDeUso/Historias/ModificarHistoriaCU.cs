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