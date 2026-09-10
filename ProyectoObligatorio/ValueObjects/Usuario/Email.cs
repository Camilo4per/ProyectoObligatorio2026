using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.ValueObjects.Usuario
{
    [Owned]
    public sealed record Email : IValidable
    {
        public string Value { get; init; }

        public Email() { }
        public Email(string text)
        {
            Value = text;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new UsuarioException("Email requerido");

            if (!Value.Contains("@") || !Value.Contains("."))
                throw new UsuarioException("Email inválido");
        }

        public override string ToString() => Value;
    }
}