using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioFinales : IRepository<Final>
    {
        public void ExisteFinal(Final aVerificar);
    }
}