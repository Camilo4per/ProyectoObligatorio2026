using ProyectoObligatorio.InterfacesDelDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAplicacion.Entidades
{
    public class Usuario : IValidable
    {
        public int Id { get; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public RolesUsuario Rol { get; set; }

        public Usuario(string nombreCompleto, string email, string nombreUsuario, string password, RolesUsuario rol)
        {
            NombreCompleto = nombreCompleto;
            Email = email;
            NombreUsuario = nombreUsuario;
            Password = password;
            Rol = rol;
        }

    }
}
