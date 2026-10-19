using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IObtenerLecturasPorId
    {
        public LecturaDTO Ejecutar(int id);
    }
}
