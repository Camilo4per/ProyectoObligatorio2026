using Dominio.Entidades;

namespace Dominio.InterfaceDeRepositorio
{
    public interface IRepositorioHistorias : IRepository<Historia>
    {
        public void ExisteHistoria(Historia aVerificar);

        // Historias publicadas para el catálogo del lector. El filtro por categoría
        // es opcional (null o <= 0 = sin filtro).
        public List<Historia> FindPublicadas(int? idCategoria);
    }
}