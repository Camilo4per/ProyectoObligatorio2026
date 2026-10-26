using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class EncontrarHistoriasPublicadasCU : IEncontrarHistoriasPublicadas
    {
        private IRepositorioHistorias repositorio;

        public EncontrarHistoriasPublicadasCU(IRepositorioHistorias repo)
        {
            repositorio = repo;
        }

        public List<HistoriaCatalogoDTO> Ejecutar(int? idCategoria)
        {
            return repositorio.FindPublicadas(idCategoria)
                .Select(HistoriaMapper.ToCatalogoDTO)
                .OrderBy(h => h.Titulo)
                .ToList();
        }
    }
}
