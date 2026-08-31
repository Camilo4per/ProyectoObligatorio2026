using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Usuario : IValidable
    {
        public int Id { get; }
        public string NombreCompleto { get; set; }
        
        public string NombreUsuario { get; set; }
        public RolesUsuario Rol { get; set; }

        // Value Objects
        public Email Email { get; set; }
        public Password Password { get; set; }


        public Usuario(string nombreCompleto, string email, string nombreUsuario, string password, RolesUsuario rol)
        {
            NombreCompleto = nombreCompleto;
            Email = email;
            NombreUsuario = nombreUsuario;
            Password = password;
            Rol = rol;
        }
        public void Validar()
        {
            if (string.IsNullOrEmpty(Email)) { throw new UsuarioException("Email no puede ser vacio"); }
        }
    }
}
