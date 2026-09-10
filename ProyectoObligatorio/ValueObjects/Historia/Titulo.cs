using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.ValueObjects.Historia
{
    [Owned]
    public sealed record TituloHistoria : IValidable
    {
        public string Valor { get; init; }

        public TituloHistoria() { }
        public TituloHistoria(string texto)
        {
            Valor = texto;
            Validar();
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