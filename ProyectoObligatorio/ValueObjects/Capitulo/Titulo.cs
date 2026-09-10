using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.ValueObjects.Capitulo
{
    [Owned]
    public sealed record Titulo : IValidable
    {
        public string Valor { get; init; }

        public Titulo() { }
        public Titulo(string texto)
        {
            Valor = texto;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Valor))
                throw new CapituloException("Título requerido");

            if (Valor.Length < 3)
                throw new CapituloException("Título debe tener al menos 3 caracteres");

            if (Valor.Length > 100)
                throw new CapituloException("Título no puede exceder los 100 caracteres");
        }

        public override string ToString() => Valor;
    }
}