using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;

namespace Dominio.ValueObjects.Historia
{
    public sealed record Titulo : IValidable
    {
        public string Valor { get; init; }

        public Titulo(string texto)
        {
            Valor = texto;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Valor))
                throw new HistoriaException("Título requerido");

            if (Valor.Length < 3)
                throw new HistoriaException("Título debe tener al menos 3 caracteres");

            if (Valor.Length > 100)
                throw new HistoriaException("Título no puede exceder los 100 caracteres");
        }

        public override string ToString() => Valor;
    }
}