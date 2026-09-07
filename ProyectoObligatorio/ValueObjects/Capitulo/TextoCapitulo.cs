using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;

namespace Dominio.ValueObjects.Capitulo
{
    public sealed record TextoCapitulo : IValidable
    {
        public string Valor { get; init; }

        public TextoCapitulo(string texto)
        {
            Valor = texto;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Valor))
                throw new CapituloException("Texto requerido");

            if (Valor.Length < 3)
                throw new CapituloException("Texto debe tener al menos 3 caracteres");

            if (Valor.Length > 2000)
                throw new CapituloException("Texto no puede exceder los 2000 caracteres");
        }

        public override string ToString() => Valor;
    }
}