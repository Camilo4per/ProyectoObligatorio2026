using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IObtenerPasoLectura
    {
        public PasoLecturaDTO Ejecutar(int idLectura, int idCapitulo, int idUsuario);
    }
}
