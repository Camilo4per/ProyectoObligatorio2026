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
        public Email Email { get; init; }
        public Password Password { get; init; }

    }
}
