using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class AgregarHistoriaCU : IAgregarHistoria
    {
        private IRepositorioHistorias repositorio;
        public AgregarHistoriaCU(IRepositorioHistorias repo)
        {
            repositorio = repo;
        }
        public void Ejecutar(HistoriaDTO dto)
        {
            repositorio.Add(HistoriaMapper.FromDTO(dto));
        }
    }
}
