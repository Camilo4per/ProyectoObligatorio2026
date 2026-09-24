using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class ObtenerHistoriaPorIdCU : IObtenerHistoriaPorId
    {
        private IRepositorioHistorias repositorio;
        public ObtenerHistoriaPorIdCU(IRepositorioHistorias repo)
        {
            repositorio = repo;
        }

        public HistoriaDTO Ejecutar(int id)
        {
            return HistoriaMapper.ToDTO(repositorio.FindById(id));
        }
    }
}