using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;

namespace Dominio.ValueObjects.Usuario
{
    public sealed record NombreCompleto : IValidable
    {
        public string Nombre { get; init; }
        public string Apellido { get; init; }

        public NombreCompleto(string nombre, string apellido)
        {
            Nombre = nombre;
            Apellido = apellido;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                throw new UsuarioException("Nombre requerido");

            if (string.IsNullOrWhiteSpace(Apellido))
                throw new UsuarioException("Apellido requerido");
        }

        public override string ToString() => $"{Nombre} {Apellido}";
    }
}