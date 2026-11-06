using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class ObtenerFinalesPorHistoriaCU : IObtenerFinalesPorHistoria
    {
        private IRepositorioHistorias repositorioHistorias;

        public ObtenerFinalesPorHistoriaCU(
            IRepositorioHistorias repositorioHistoriasCu)
        {
            repositorioHistorias = repositorioHistoriasCu;
        }

        public FinalesHistoriaDTO Ejecutar(int idHistoria)
        {
            if (idHistoria <= 0)
            {
                throw new HistoriaException(
                    "El identificador de la historia no es válido.");
            }

            Historia historia = repositorioHistorias.FindById(idHistoria);
            List<Final> finales = historia.Capitulos.OfType<Final>().ToList();
            int mayorCantidad = finales.Any()
                ? finales.Max(final => final.CantLlegaron.Valor)
                : 0;

            return new FinalesHistoriaDTO
            {
                IdHistoria = historia.Id,
                TituloHistoria = historia.Titulo.Valor,
                Finales = finales
                    .Select(final => new FinalFrecuenciaDTO
                    {
                        IdFinal = final.Id,
                        Titulo = final.Titulo.Valor,
                        CantidadVecesAlcanzado = final.CantLlegaron.Valor,
                        EsMasFrecuente = mayorCantidad > 0 && final.CantLlegaron.Valor == mayorCantidad
                    })
                    .OrderByDescending(final => final.CantidadVecesAlcanzado)
                    .ThenBy(final => final.Titulo)
                    .ToList()
            };
        }
    }
}
