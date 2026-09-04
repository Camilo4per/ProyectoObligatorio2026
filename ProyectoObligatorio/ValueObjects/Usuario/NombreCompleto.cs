
namespace Dominio.ValueObjects.Usuario
{
    public sealed record NombreCompleto
    {
        public string Nombre { get; init; }
        public string Apellido { get; init; }

        public NombreCompleto(string nombre, string apellido)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre requerido", nameof(nombre));

            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("Apellido requerido", nameof(apellido));

            Nombre = nombre;
            Apellido = apellido;
        }

        public override string ToString() => $"{Nombre} {Apellido}";
    }
}
