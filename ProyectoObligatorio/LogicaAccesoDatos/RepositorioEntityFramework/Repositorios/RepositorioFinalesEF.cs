using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using Microsoft.EntityFrameworkCore;

namespace LogicaAccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioFinalesEF : IRepositorioFinales
    {
        private DominioContext _context;

        public RepositorioFinalesEF(DominioContext context)
        {
            _context = context;
        }

        public void Add(Final nuevo)
        {
            if (nuevo == null) { throw new FinalException("El capitulo final esta vacio"); }
            nuevo.Validar();
            ExisteFinal(nuevo);
            _context.Finales.Add(nuevo);
            _context.SaveChanges();
        }

        public void ExisteFinal(Final aVerificar)
        {
            if (_context.Finales.Any(f => f.Id == aVerificar.Id))
            {
                throw new FinalException("Capitulo final ya existente");
            }
        }

        public void Remove(int id)
        {
            _context.Capitulos.Where(c => c.Id == id).ExecuteDelete();
        }

        public void Update(Final nuevo)
        {
            if (nuevo == null) { throw new FinalException("El capitulo final esta vacio"); }
            nuevo.Validar();
            _context.Finales.Update(nuevo);
            _context.SaveChanges();
        }

        public List<Final> FindAll()
        {
            return _context.Finales.ToList();
        }

        public Final FindById(int id)
        {
            return _context.Finales.Where(f => f.Id == id).FirstOrDefault()
                ?? throw new FinalException("Capitulo final inexistente");
        }
    }
}