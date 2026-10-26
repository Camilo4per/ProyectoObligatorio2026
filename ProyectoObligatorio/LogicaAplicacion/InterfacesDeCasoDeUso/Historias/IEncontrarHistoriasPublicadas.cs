using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Historias
{
    public interface IEncontrarHistoriasPublicadas
    {
        public List<HistoriaCatalogoDTO> Ejecutar(int? idCategoria);
    }
}
