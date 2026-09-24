using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Dominio.ValueObjects.Capitulo
{
    [Owned]
    public sealed record TituloCapitulo : IValidable
    {
        public string Valor { get; init; }

        public TituloCapitulo() { }
        public TituloCapitulo(string texto)
        {
            Valor = texto;
            Validar();
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