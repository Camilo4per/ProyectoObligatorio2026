using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ObtenerRankingHistoriasCU : IObtenerRankingHistorias
    {
        private IRepositorioLecturas repositorio;

        public ObtenerRankingHistoriasCU(IRepositorioLecturas repo)
        {
            repositorio = repo;
        }

        public List<HistoriaRankingDTO> Ejecutar()
        {
            List<Lectura> lecturasCompletadas =
                repositorio.FindAllCompletadas();

            return lecturasCompletadas
                .Where(l => l.Historia != null)
                // GroupBy para agrupar las lecturas por idHistoria y Titulo
                .GroupBy(l => new
                {
                    IdHistoria = l.idHistoria!.Value,
                    Titulo = l.Historia.Titulo.Valor
                })
                // Select del GroupBy para crear un objeto HistoriaRankingDTO con la información necesaria
                .Select(grupo => new HistoriaRankingDTO
                {
                    IdHistoria = grupo.Key.IdHistoria,
                    Titulo = grupo.Key.Titulo,
                    CantidadLecturasCompletadas = grupo.Count()
                })
                .OrderByDescending(h => h.CantidadLecturasCompletadas)
                .ThenBy(h => h.Titulo)
                .ToList();
        }
    }
}
