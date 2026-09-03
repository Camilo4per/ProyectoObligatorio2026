using Dominio.Entidades;
using Dominio.Excepciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LogicaAccesoDatos.RepositorioEnMemoria
{
    public class RepositorioUsuarios
    {
        private static List<Usuario> usuarios = new List<Usuario>();

        public void Add(Usuario nuevo)
        {
            if (nuevo == null) { throw new UsuarioException("El usuario esta vacio"); }
            nuevo.Validar();
            UsuarioUnico(nuevo);
            RepositorioUsuarios.usuarios.Add(nuevo);
        }

        public void UsuarioUnico(Usuario usuario)
        {
            foreach (Usuario item in usuarios)
            {
                if (item.Email == usuario.Email)
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
