using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class AgregarHistoriaCU
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
