using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.ValueObjects.Usuario
{
    [Owned]
    public sealed record NombreCompleto : IValidable
    {
        public string Nombre { get; init; }
        public string Apellido { get; init; }

        public NombreCompleto() { }
        public NombreCompleto(string nombre, string apellido)
        {
            Nombre = nombre;
            Apellido = apellido;
            Validar();
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                throw new UsuarioException("Nombre requerido");

            if (string.IsNullOrWhiteSpace(Apellido))
                throw new UsuarioException("Apellido requerido");

            if (Nombre.Length < 2 || Nombre.Length > 20)
                throw new UsuarioException("Nombre debe tener entre 2 y 20 caracteres");

            if (Apellido.Length < 2 || Apellido.Length > 20)
                throw new UsuarioException("Apellido debe tener entre 2 y 20 caracteres");
        }

        public override string ToString() => $"{Nombre} {Apellido}";
    }
}