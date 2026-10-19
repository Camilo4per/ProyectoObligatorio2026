using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class RankingHistoriasCU : IRankingHistorias
    {
        private IRepositorioLecturas _repositorio;

        public RankingHistoriasCU(IRepositorioLecturas repositorio)
        {
            _repositorio = repositorio;
        }

        // RF09: historias con al menos una lectura completada, ordenadas por
        // cantidad de lecturas completadas de mayor a menor. Las historias
        // nunca leídas (o solo abandonadas) no aparecen porque agrupamos
        // únicamente las lecturas finalizadas.
        public List<RankingHistoriaDTO> Ejecutar()
        {
            List<Lectura> completadas = _repositorio.FindAllCompletadas();

            return completadas
                .GroupBy(l => l.Historia)
                .Select(grupo => new RankingHistoriaDTO
                {
                    IdHistoria = grupo.Key.Id,
                    Titulo = grupo.Key.Titulo.Valor,
                    CantLecturasCompletadas = grupo.Count()
                })
                .OrderByDescending(r => r.CantLecturasCompletadas)
                .ThenBy(r => r.Titulo)
                .ToList();
        }
    }
}
