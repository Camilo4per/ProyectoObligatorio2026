using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;

namespace Dominio.ValueObjects.Opcion
{
    public sealed record TextoOpcion : IValidable
    {
        public string Valor { get; init; }

        public TextoOpcion(string texto)
        {
            Valor = texto;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Valor))
                throw new OpcionException("Texto requerido");

            if (Valor.Length < 3)
                throw new OpcionException("Texto debe tener al menos 3 caracteres");

            if (Valor.Length > 2000)
                throw new OpcionException("Texto no puede exceder los 2000 caracteres");
        }

        public override string ToString() => Valor;
    }
}