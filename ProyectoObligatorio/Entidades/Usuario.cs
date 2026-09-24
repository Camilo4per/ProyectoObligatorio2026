using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Usuario;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
{
    public class Usuario : IValidable
    {
        #region Atributos
        public int Id { get; set; }

        public string NombreUsuario { get; set; }

        public RolesUsuario Rol { get; set; }

        public NombreCompleto NombreCompleto { get; init; }

        public Email Email { get; init; }

        public Password Password { get; init; }

        #endregion

        #region Validaciones
        public void Validar()
        {
            Password.Validar();
            Email.Validar();
            NombreCompleto.Validar();
        }

        public void ValidarNombreUsuario()
        {
            if (string.IsNullOrEmpty(NombreUsuario)) { throw new UsuarioException("Nombre de usuario no puede ser vacio"); }
            if (string.IsNullOrWhiteSpace(NombreUsuario)) { throw new UsuarioException("Nombre de usuario no puede contener solo espacios en blanco"); }
            if (NombreUsuario.Length < 3) { throw new UsuarioException("Nombre de usuario debe tener al menos 3 caracteres"); }
        }

        #endregion
    }
}
