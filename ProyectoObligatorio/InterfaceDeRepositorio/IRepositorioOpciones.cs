using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioOpciones : IRepository<Opcion>
    {
        public void ExisteOpcion(Opcion aVerificar);
    }
}