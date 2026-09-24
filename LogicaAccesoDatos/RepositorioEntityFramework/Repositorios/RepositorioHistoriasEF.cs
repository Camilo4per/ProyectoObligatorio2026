using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using Microsoft.EntityFrameworkCore;

namespace LogicaAccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioHistoriasEF : IRepositorioHistorias
    {
        private DominioContext _context;

        public RepositorioHistoriasEF(DominioContext context)
        {
            _context = context;
        }

        public void Add(Historia nuevo)
        {
            if (nuevo == null) { throw new HistoriaException("La historia esta vacia"); }
            nuevo.Validar();
            ExisteHistoria(nuevo);
            AdjuntarCategorias(nuevo);
            _context.Historias.Add(nuevo);
            _context.SaveChanges();
        }

        public void ExisteHistoria(Historia aVerificar)
        {
            if (_context.Historias.Any(h => h.Id == aVerificar.Id))
            {
                throw new HistoriaException("Historia ya existente");
            }
        }

        public void Remove(int id)
        {
            _context.Historias.Remove(new Historia { Id = id });
            _context.SaveChanges();
        }

        public void Update(Historia nuevo)
        {
            if (nuevo == null) { throw new HistoriaException("La historia esta vacia"); }
            nuevo.Validar();
            AdjuntarCategorias(nuevo);
            _context.Historias.Update(nuevo);
            _context.SaveChanges();
        }

        private void AdjuntarCategorias(Historia historia)
        {
            List<Categoria> ligadas = new List<Categoria>();
            foreach (Categoria categoria in historia.Categorias)
            {
                Categoria existente = _context.Categorias.Find(categoria.Id)
                    ?? throw new CategoriaException("Categoría inexistente");
                ligadas.Add(existente);
            }

            historia.Categorias = ligadas;
        }

        public List<Historia> FindAll()
        {
            return _context.Historias
                .Include(h => h.Capitulos)
                .Include(h => h.Categorias)
                .Include(h => h.CapituloInicial)
                .ToList();
        }

        public Historia FindById(int id)
        {
            return _context.Historias
                .Include(h => h.Capitulos)
                .Include(h => h.Categorias)
                .Include(h => h.CapituloInicial)
                .Where(h => h.Id == id)
                .FirstOrDefault()
                ?? throw new HistoriaException("Historia inexistente");
        }
    }
}