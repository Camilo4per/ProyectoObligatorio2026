using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using Microsoft.EntityFrameworkCore;

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
            _context.Lecturas.Remove(FindById(id));
            _context.SaveChanges();
        }

        public void Update(Lectura nueva)
        {
            if (nueva == null) { throw new LecturaException("La lectura esta vacia"); }
            nueva.Validar();

            // Solo se actualizan las columnas modificables de la lectura (final alcanzado y
            // fecha de fin). Es tracker-free a proposito: si se usara Update(grafo) se volverian
            // a marcar Usuario, Historia y sus capitulos como Modified y EF intentaria
            // volver a guardarlos.
            _context.Lecturas.Where(l => l.Id == nueva.Id).ExecuteUpdate(s => s
                .SetProperty(l => l.idFinal, nueva.idFinal)
                .SetProperty(l => l.FechaFin, nueva.FechaFin));
        }

        public List<Lectura> FindAll()
        {
            // HistoriaMapper.ToDTO recorre los capitulos y las categorias de la historia:
            // sin los ThenInclude, GET /api/Lectura lanza NullReferenceException.
            return _context.Lecturas
                .Include(l => l.Usuario)
                .Include(l => l.Historia).ThenInclude(h => h.Categorias)
                .Include(l => l.Historia).ThenInclude(h => h.Capitulos)
                .Include(l => l.Historia).ThenInclude(h => h.CapituloInicial)
                .Include(l => l.FinalAlcanzado)
                .ToList();
        }

        // Lecturas finalizadas (con FechaFin, lo que por Validar() implica final alcanzado)
        // con su historia cargada, para el ranking de historias más leídas (RF09).
        public List<Lectura> FindAllCompletadas()
        {
            return _context.Lecturas
                .Include(l => l.Usuario)
                .Include(l => l.Historia)
                .Where(l => l.FechaFin != null)
                .ToList();
        }

        public List<Lectura> FindByUsuarioAndMonth(int idUsuario, int mes, int anio)
        {
            DateTime desde = new DateTime(anio, mes, 1);
            DateTime hasta = desde.AddMonths(1);

            return _context.Lecturas
                .AsNoTracking()
                .Include(l => l.Historia)
                .Include(l => l.FinalAlcanzado)
                .Where(l => l.idUsuario == idUsuario &&
                            l.FechaInicio >= desde &&
                            l.FechaInicio < hasta)
                .OrderByDescending(l => l.FechaInicio)
                .ToList();
        }

        public Lectura FindById(int id)
        {
            // HistoriaMapper.ToDTO necesita el grafo de la historia (capitulos y categorias);
            // sin estos Include, GET /api/Lectura/{id} tambien lanza NullReferenceException.
            return _context.Lecturas
                .Include(l => l.Usuario)
                .Include(l => l.Historia).ThenInclude(h => h.Categorias)
                .Include(l => l.Historia).ThenInclude(h => h.Capitulos)
                .Include(l => l.Historia).ThenInclude(h => h.CapituloInicial)
                .Include(l => l.FinalAlcanzado)
                .Where(l => l.Id == id).FirstOrDefault()
                ?? throw new LecturaException("Lectura inexistente");
        }
    }
}