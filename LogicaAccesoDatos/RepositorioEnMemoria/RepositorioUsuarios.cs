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
        public List<Usuario> usuarios;

        public void Add(Usuario nuevo) 
        {
            if (nuevo == null) throw new UsuarioException("Usuario ya existe");
            nuevo.Validar();
            ExisteUsuario(nuevo);
            RepositorioUsuarios.usuarios.Add(nuevo);
        } 
    }
}
