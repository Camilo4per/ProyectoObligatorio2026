
namespace Dominio.ValueObjects.Categoria
{
    public sealed record Nombre
    {
        public string Valor { get; init; }

        public Nombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre requerido", nameof(nombre));

            if (nombre.Length < 3)
                throw new ArgumentException("Nombre debe tener al menos 3 caracteres", nameof(nombre));

            if (nombre.Length > 30)
                throw new ArgumentException("Nombre no puede exceder los 30 caracteres", nameof(nombre));

            Valor = nombre;
        }

        public override string ToString() => Valor;
    }
}