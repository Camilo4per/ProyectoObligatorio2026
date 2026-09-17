using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioCategoriasEF : IRepositorioCategorias
    {
        private DominioContext _context;

        public RepositorioCategoriasEF(DominioContext context)
        {
            _context = context;
        }

        public void Add(Categoria nuevo)
        {
            if (nuevo == null) { throw new CategoriaException("La categoria esta vacia"); }
            nuevo.Validar();
            ExisteCategoria(nuevo);
            _context.Categorias.Add(nuevo);
            _context.SaveChanges();
        }

        public void ExisteCategoria(Categoria aVerificar)
        {
            if (_context.Categorias.Any(c => c.Nombre.Valor == aVerificar.Nombre.Valor))
            {
                throw new CategoriaException("Categoria ya existente");
            }
        }

        public void Remove(int id)
        {
            _context.Categorias.Remove(new Categoria { Id = id });
            _context.SaveChanges();
        }

        public void Update(Categoria nuevo)
        {
            nuevo.Validar();
            _context.Categorias.Update(nuevo);
            _context.SaveChanges();
        }
        public Categoria FindById(int id)
        {
            return _context.Categorias.Where(c => c.Id == id).FirstOrDefault()
                ?? throw new CategoriaException("Categoria inexistente");
        }

        public List<Categoria> FindAll()
        {
            return _context.Categorias.ToList();
        }
    }
}