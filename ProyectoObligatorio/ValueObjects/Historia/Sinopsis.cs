using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;

namespace Dominio.ValueObjects.Historia
{
    public sealed record Sinopsis : IValidable
    {
        public string Valor { get; init; }

        public Sinopsis(string texto)
        {
            Valor = texto;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Valor))
                throw new HistoriaException("Sinopsis requerida");

            if (Valor.Length < 10)
                throw new HistoriaException("Sinopsis debe tener al menos 10 caracteres");

            if (Valor.Length > 500)
                throw new HistoriaException("Sinopsis no puede exceder los 500 caracteres");
        }

        public override string ToString() => Valor;
    }
}