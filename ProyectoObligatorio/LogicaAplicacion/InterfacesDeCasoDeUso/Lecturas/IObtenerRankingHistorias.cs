using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IObtenerRankingHistorias
    {
        List<HistoriaRankingDTO> Ejecutar();
    }
}
