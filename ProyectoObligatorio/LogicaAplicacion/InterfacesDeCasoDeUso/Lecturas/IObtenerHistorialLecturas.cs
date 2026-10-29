using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IObtenerHistorialLecturas
    {
        List<HistorialLecturaDTO> Ejecutar(int idUsuario, int mes, int anio);
    }
}
