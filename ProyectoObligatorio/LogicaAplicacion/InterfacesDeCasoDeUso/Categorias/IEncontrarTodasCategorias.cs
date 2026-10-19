using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesDeCasoDeUso.Categorias
{
    public interface IEncontrarTodasCategorias
    {
        public List<CategoriaDTO> Ejecutar();
    }
}