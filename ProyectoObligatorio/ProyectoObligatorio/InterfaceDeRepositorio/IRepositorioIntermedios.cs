using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioIntermedios : IRepository<Intermedio>
    {
        public void ExisteIntermedio(Intermedio aVerificar);
    }
}