using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using Microsoft.EntityFrameworkCore;

namespace LogicaAccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioIntermediosEF : IRepositorioIntermedios
    {
        private DominioContext _context;

        public RepositorioIntermediosEF(DominioContext context)
        {
            _context = context;
        }

        public void Add(Intermedio nuevo)
        {
            if (nuevo == null) { throw new IntermedioException("El capitulo intermedio esta vacio"); }
            nuevo.Validar();
            ExisteIntermedio(nuevo);
            _context.Intermedios.Add(nuevo);
            _context.SaveChanges();
        }

        public void ExisteIntermedio(Intermedio aVerificar)
        {
            if (_context.Intermedios.Any(i => i.Id == aVerificar.Id))
            {
                throw new IntermedioException("Capitulo intermedio ya existente");
            }
        }

        public void Remove(int id)
        {
            _context.Capitulos.Where(c => c.Id == id).ExecuteDelete();
            _context.SaveChanges();
        }

        public void Update(Intermedio nuevo)
        {
            if (nuevo == null) { throw new IntermedioException("El capitulo intermedio esta vacio"); }
            nuevo.Validar();
            _context.Intermedios.Update(nuevo);
            _context.SaveChanges();
        }

        public List<Intermedio> FindAll()
        {
            return _context.Intermedios.ToList();
        }

        public Intermedio FindById(int id)
        {
            return _context.Intermedios.Where(i => i.Id == id).FirstOrDefault()
                ?? throw new IntermedioException("Capitulo intermedio inexistente");
        }
    }
}