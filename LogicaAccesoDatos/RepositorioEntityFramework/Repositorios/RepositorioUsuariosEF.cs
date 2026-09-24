using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using Dominio.ValueObjects.Usuario;

namespace LogicaAccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioUsuariosEF : IRepositorioUsuarios
    {

        private DominioContext _context;

        public RepositorioUsuariosEF(DominioContext context)
        {
            _context = context;
        }

        public void Add(Usuario nuevo)
        {
            if (nuevo == null) { throw new UsuarioException("El usuario esta vacio"); }
            nuevo.Validar();
            ExisteUsuario(nuevo);
            _context.Usuarios.Add(nuevo);
            _context.SaveChanges();
        }

        public void ExisteUsuario(Usuario aVerificar)
        {
            if (_context.Usuarios.Any(u => u.NombreUsuario == aVerificar.NombreUsuario))
            {
                throw new UsuarioException("Un usuario con el mismo nombre ya existe.");
            }
        }

        public void Remove(int id)
        {
            _context.Usuarios.Remove(new Usuario { Id = id });
            _context.SaveChanges();
        }

        public void Update(Usuario nuevo)
        {
            if (nuevo == null) { throw new UsuarioException("El usuario esta vacio"); }
            nuevo.Validar();
            _context.Usuarios.Update(nuevo);
            _context.SaveChanges();
        }
        public List<Usuario> FindAll()
        {
            return _context.Usuarios.ToList();
        }

        public Usuario FindById(int id)
        {
            // Evalúa el resultado de la izquierda. Si es distinto de null, lo retorna directamente.
            // Si no es null ?? Si es null
            return _context.Usuarios.Where(u => u.Id == id).FirstOrDefault()
                ?? throw new UsuarioException("Usuario inexistente");
        }

        public Usuario? FindByNombreUsuario(string nombreUsuario)
        {
            return _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);
        }
    }
}
