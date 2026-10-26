using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IAvanzarLectura
    {
        public PasoLecturaDTO Ejecutar(int idLectura, AvanzarLecturaDTO dto, bool esAdmin);
    }
}
