
namespace Dominio.ValueObjects.Historia
{
    public sealed record Sinopsis
    {
        public string Valor { get; init; }

        public Sinopsis(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Sinopsis requerida", nameof(texto));

            if (texto.Length < 10)
                throw new ArgumentException("Sinopsis debe tener al menos 10 caracteres", nameof(texto));

            if (texto.Length > 500)
                throw new ArgumentException("Sinopsis no puede exceder los 500 caracteres", nameof(texto));

            Valor = texto;
        }

        public override string ToString() => Valor;
    }
}