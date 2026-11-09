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

        public void Add(Historia unaHistoria)
        {
            if (unaHistoria == null) { throw new HistoriaException("La historia esta vacia"); }
            unaHistoria.Validar();
            ExisteHistoria(unaHistoria);
            AdjuntarCategorias(unaHistoria);

            // Historia.Id y Capitulo.Id los genera la BD, y las FKs idCapituloInicial
            // y idHistoria se apuntan entre sí: en un solo SaveChanges no hay orden de
            // INSERT posible (dependencia circular). Guardo la historia con sus capitulos
            // sin el FK al inicial, y en un segundo SaveChanges seteo idCapituloInicial
            // con el Id ya generado.
            Capitulo inicial = unaHistoria.CapituloInicial;
            unaHistoria.CapituloInicial = null!;

            try
            {
                _context.Historias.Add(unaHistoria);
                _context.SaveChanges();
            }
            finally
            {
                // Se restaura siempre la referencia original, incluso si SaveChanges falla.
                unaHistoria.CapituloInicial = inicial;
            }

            // Ahora que el capítulo tiene su ID generado, se puede establecer la FK
            // de la historia hacia su capítulo inicial.
            unaHistoria.idCapituloInicial = inicial.Id;

            // La historia continúa siendo rastreada por el DbContext.
            // Por eso SaveChanges detecta el cambio y ejecuta el UPDATE correspondiente.
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
            // DELETE directo de la fila: se saltea el change tracker (no tiene que ordenar
            // el borrado del ciclo de FKs) y la base resuelve la cascada: capitulos por
            // idHistoria (Cascade) y los joins de CategoriaHistoria.
            _context.Historias.Where(h => h.Id == id).ExecuteDelete();
        }

        public void Update(Historia nuevo)
        {
            if (nuevo == null) { throw new HistoriaException("La historia esta vacia"); }
            nuevo.Validar();

            // Patron EF estandar: cargo la version trackeada y muto sus valores; EF calcula
            // los cambios (incluida la tabla intermedia CategoriaHistoria) sin re-adjuntar
            // un grafo que pueda chocar con el tracking o duplicar joins.
            Historia existente = _context.Historias
                .Include(h => h.Capitulos)
                .Include(h => h.Categorias)
                .Include(h => h.CapituloInicial)
                .FirstOrDefault(h => h.Id == nuevo.Id)
                ?? throw new HistoriaException("Historia inexistente");

            existente.Titulo = nuevo.Titulo;
            existente.Sinopsis = nuevo.Sinopsis;
            existente.Estado = nuevo.Estado;

            // Si la historia solo tiene el capitulo inicial, permito modificar su titulo y texto.
            if (existente.Capitulos.Count <= 1 && nuevo.CapituloInicial != null && existente.CapituloInicial != null)
            {
                existente.CapituloInicial.Titulo = nuevo.CapituloInicial.Titulo;
                existente.CapituloInicial.Texto = nuevo.CapituloInicial.Texto;
            }

            existente.Categorias.Clear();
            foreach (Categoria categoria in nuevo.Categorias)
            {
                existente.Categorias.Add(_context.Categorias.Find(categoria.Id)
                    ?? throw new CategoriaException("Categoria inexistente"));
            }

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
            List<Historia> historias = _context.Historias
                .Include(h => h.Capitulos)
                .Include(h => h.Categorias)
                .Include(h => h.CapituloInicial)
                .ToList();

            // Cargar los intermedios y sus opciones para todas las historias encontradas.
            List<int> ids = historias.Select(h => h.Id).ToList();
            if (ids.Count > 0)
            {
                _context.Intermedios
                    .Where(i => i.idHistoria.HasValue && ids.Contains(i.idHistoria.Value))
                    .Include(i => i.Opciones)
                    .ToList();
            }

            return historias;
        }

        // Catálogo del lector: sólo publicadas, con las categorías para el filtro.
        public List<Historia> FindPublicadas(int? idCategoria)
        {
            return _context.Historias
                .Include(h => h.Categorias)
                .Where(h => h.Estado == EstadoHistoria.Publicada && (
                    !idCategoria.HasValue || idCategoria.Value <= 0 || h.Categorias.Any(c => c.Id == idCategoria.Value))
                    )
                .ToList();
        }

        public Historia FindById(int id)
        {
            Historia historia = _context.Historias
                .Include(h => h.Categorias)
                .Include(h => h.CapituloInicial)
                .Include(h => h.Capitulos)
                    .ThenInclude(c => (c as Intermedio)!.Opciones)
                .FirstOrDefault(h => h.Id == id)
                ?? throw new HistoriaException("Historia inexistente");

            return historia;
        }
    }
}