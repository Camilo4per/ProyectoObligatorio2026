
namespace Dominio.ValueObjects.Historia
{
    public sealed record Titulo
    {
        public string Valor { get; init; }

        public Titulo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Título requerido", nameof(texto));

            if (texto.Length < 3)
                throw new ArgumentException("Título debe tener al menos 3 caracteres", nameof(texto));

            if (texto.Length > 100)
                throw new ArgumentException("Título no puede exceder los 100 caracteres", nameof(texto));

            Valor = texto;
        }

        public override string ToString() => Valor;
    }
}