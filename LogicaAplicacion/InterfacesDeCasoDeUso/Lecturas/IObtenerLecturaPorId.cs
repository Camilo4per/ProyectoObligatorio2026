using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IObtenerLecturaPorId
    {
        public LecturaDTO Ejecutar(int id);
    }
}