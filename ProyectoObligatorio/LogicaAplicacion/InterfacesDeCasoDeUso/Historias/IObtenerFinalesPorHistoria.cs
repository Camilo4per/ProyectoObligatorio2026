using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias
{
    public interface IObtenerFinalesPorHistoria
    {
        FinalesHistoriaDTO Ejecutar(int idHistoria);
    }
}
