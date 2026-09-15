using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEntityFramework.Repositorios
{
    public class RepositorioUsuariosEF : IRepositorioUsuarios
    {

        private DominioContext _context;

        public RepositorioUsuariosEF(DominioContext context)
        {
            _context = new DominioContext();
        }

        public void Add(Usuario nuevo)
        {
            if (nuevo == null) { throw new UsuarioException("El usuario esta vacio"); }
            ExisteUsuario(nuevo);
            nuevo.Validar();
            _context.Usuarios.Add(nuevo);
            _context.SaveChanges();
        }

        public void ExisteUsuario(Usuario aVerificar)
        {
            if (_context.Usuarios.Any(u => u.NombreUsuario == aVerificar.NombreUsuario))
            {
                throw new UsuarioException("El usuario ya existe.");
            }
        }

        public List<Usuario> FindAll()
        {
            return _context.Usuarios.ToList();
        }

        public Usuario FindById(int id)
        {
            return _context.Usuarios.Where(u => u.Id == id).FirstOrDefault();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Usuario nuevo)
        {
            throw new NotImplementedException();
        }
    }
}
