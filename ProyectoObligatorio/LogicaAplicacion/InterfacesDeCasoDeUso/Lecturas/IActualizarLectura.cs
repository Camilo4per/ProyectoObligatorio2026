using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IActualizarLectura
    {
        public void Ejecutar(int id, LecturaDTO dto);
    }
}
