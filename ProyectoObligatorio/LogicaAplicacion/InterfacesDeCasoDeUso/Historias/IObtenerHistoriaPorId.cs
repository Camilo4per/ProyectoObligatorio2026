using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias
{
    public interface IObtenerHistoriaPorId
    {
        public HistoriaDTO Ejecutar(int id);
    }
}