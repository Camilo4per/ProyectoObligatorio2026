using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class EncontrarTodasLasHistoriasCU : IEncontrarTodasLasHistorias
    {
        private IRepositorioHistorias repositorio;
        public EncontrarTodasLasHistoriasCU(IRepositorioHistorias repo)
        {
            repositorio = repo;
        }

        public List<HistoriaDTO> Ejecutar()
        {
            List<Historia> historias = repositorio.FindAll();
            List<HistoriaDTO> aRetornar = new List<HistoriaDTO>();
            foreach (Historia historia in historias)
            {
                aRetornar.Add(HistoriaMapper.ToDTO(historia));
            }
            return aRetornar;
        }
    }
}