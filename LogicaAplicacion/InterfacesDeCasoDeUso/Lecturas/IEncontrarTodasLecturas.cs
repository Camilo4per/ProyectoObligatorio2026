using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas
{
    public interface IEncontrarTodasLecturas
    {
        public List<LecturaDTO> Ejecutar();
    }
}