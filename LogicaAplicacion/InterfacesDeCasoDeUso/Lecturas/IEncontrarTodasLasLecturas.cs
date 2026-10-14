using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IEncontrarTodasLasLecturas
    {
        public List<LecturaDTO> Ejecutar();
    }
}
