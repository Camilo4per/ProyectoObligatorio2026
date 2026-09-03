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
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public RolesUsuario Rol { get; set; }

        // Value Objects
        public NombreCompleto NombreCompleto { get; init; }
        public Email Email { get; init; }
        public Password Password { get; init; }

        // Valido unicamente el nombre de usuario, ya que es el unico campo obligatorio para crear un usuario.
        public void Validar()
        {
            if (string.IsNullOrEmpty(NombreUsuario)) { throw new UsuarioException("Nombre de usuario no puede ser vacio"); }
            if (string.IsNullOrWhiteSpace(NombreUsuario)) { throw new UsuarioException("Nombre de usuario no puede contener solo espacios en blanco"); }
            if (NombreUsuario.Length < 3) { throw new UsuarioException("Nombre de usuario debe tener al menos 3 caracteres"); }
        }
    }
}
