using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias
{
    public interface IEncontrarTodasHistorias
    {
        public List<HistoriaDTO> Ejecutar();
    }
}