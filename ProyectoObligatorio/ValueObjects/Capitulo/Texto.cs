
namespace Dominio.ValueObjects.Capitulo
{
    public sealed record Texto
    {
        public string Valor { get; init; }

        public Texto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Texto requerido", nameof(texto));

            if (texto.Length < 3)
                throw new ArgumentException("Texto debe tener al menos 3 caracteres", nameof(texto));

            if (texto.Length > 2000)
                throw new ArgumentException("Texto no puede exceder los 2000 caracteres", nameof(texto));

            Valor = texto;
        }

        public override string ToString() => Valor;
    }
}