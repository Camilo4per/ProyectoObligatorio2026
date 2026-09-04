using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioUsuarios : IRepository<Usuario>
    {
        public void ExisteUsuario(Usuario aVerificar);
    }
}
