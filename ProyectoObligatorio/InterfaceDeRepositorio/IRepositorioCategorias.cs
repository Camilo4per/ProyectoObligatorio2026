using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioCategorias : IRepository<Categoria>
    {
        public void ExisteCategoria(Categoria aVerificar);
    }
}