using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioLecturasEF : IRepositorioLecturas
    {
        private DominioContext _context;

        public RepositorioLecturasEF(DominioContext context)
        {
            _context = context;
        }

        public void Add(Lectura nueva)
        {
            if (nueva == null) { throw new LecturaException("La lectura esta vacia"); }
            nueva.Validar();
            ExisteLectura(nueva);
            _context.Lecturas.Add(nueva);
            _context.SaveChanges();
        }

        public void ExisteLectura(Lectura aVerificar)
        {
            if (_context.Lecturas.Any(l => l.Usuario.Id == aVerificar.Usuario.Id &&
                                           l.Historia.Id == aVerificar.Historia.Id &&
                                           l.FechaInicio == aVerificar.FechaInicio))
            {
                throw new LecturaException("Lectura ya existente");
            }
        }

        // True si hay lecturas ligadas a la historia, ya sea por idHistoria (directa)
        // o porque terminaron en un final que pertenece a esa historia (idFinal).
        public bool TieneLecturasDeHistoria(int idHistoria)
        {
            return _context.Lecturas.Any(l =>
                l.idHistoria == idHistoria ||
                (l.idFinal != null && l.FinalAlcanzado!.idHistoria == idHistoria));
        }

        public void Remove(int id)
        {
            _context.Lecturas.Remove(new Lectura { Id = id });
            _context.SaveChanges();
        }

        public void Update(Lectura nueva)
        {
            if (nueva == null) { throw new LecturaException("La lectura esta vacia"); }
            nueva.Validar();
            _context.Lecturas.Update(nueva);
            _context.SaveChanges();
        }

        public List<Lectura> FindAll()
        {
            return _context.Lecturas.ToList();
        }

        public Lectura FindById(int id)
        {
            return _context.Lecturas.Where(l => l.Id == id).FirstOrDefault()
                ?? throw new LecturaException("Lectura inexistente");
        }
    }
}