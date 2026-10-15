using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class AgregarHistoriaCU : IAgregarHistoria
    {
        private IRepositorioHistorias repositorioHistorias;
        private IRepositorioCategorias repositorioCategorias;
        public AgregarHistoriaCU(IRepositorioHistorias repoHistorias, IRepositorioCategorias repoCategorias)
        {
            repositorioHistorias = repoHistorias;
            repositorioCategorias = repoCategorias;
        }

        public void Ejecutar(HistoriaDTO dto)
        {
            dto.Estado = (int) EstadoHistoria.Borrador;

            if (dto.CapituloInicial == null)
                throw new HistoriaException("La historia debe crearse con un capítulo inicial");
            dto.CapituloInicial.Tipo = (int) TipoCapitulo.Intermedio;
            dto.CapituloInicial.Opciones = null;
            dto.Capitulos = new List<CapituloDTO> { dto.CapituloInicial };

            dto.Categorias = ResolverCategorias(dto.CategoriaIds);

            repositorioHistorias.Add(HistoriaMapper.FromDTO(dto));
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