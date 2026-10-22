using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias
{
    public interface IEncontrarTodasLasHistorias
    {
        public List<HistoriaDTO> Ejecutar();
    }
}