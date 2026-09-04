using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioOpcion : IRepository<Opcion>
    {
        public void ExisteOpcion(Opcion aVerificar);
    }
}