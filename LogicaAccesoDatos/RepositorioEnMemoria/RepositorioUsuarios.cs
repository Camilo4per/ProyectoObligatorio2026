using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private static List<Usuario> usuarios = new List<Usuario>();

        public void Add(Usuario nuevo)
        {
            if (nuevo == null) { throw new UsuarioException("El usuario esta vacio"); }
            nuevo.Validar();
            ExisteUsuario(nuevo);
            usuarios.Add(nuevo);
        }

        public void ExisteUsuario(Usuario aVerificar)
        {
            foreach (Usuario item in usuarios)
            {
                if (item.Email == aVerificar.Email)
                {
                    throw new UsuarioException("Usuario ya existente");
                }
            }
        }

        public Usuario FindById(int id)
        {
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Id == id) return usuario;
            }

            throw new UsuarioException("Usuario inexistente");
        }

        public List<Usuario> FindAll()
        {
            return new List<Usuario>(usuarios);
        }

        public void Update(Usuario nuevo)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
