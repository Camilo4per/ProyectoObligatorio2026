using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioHistorias : IRepository<Historia>
    {
        public void ExisteHistoria(Historia aVerificar);
    }
}