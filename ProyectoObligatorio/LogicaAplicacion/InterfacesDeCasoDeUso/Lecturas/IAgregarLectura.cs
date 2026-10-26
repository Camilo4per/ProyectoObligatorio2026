using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IAgregarLectura
    {
        public LecturaIniciadaDTO Ejecutar(IniciarLecturaDTO dto);
    }
}
