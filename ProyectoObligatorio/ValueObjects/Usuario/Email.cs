using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ValueObjects.Usuario
{
    public sealed record Email
    {
        public string Value { get; init; }

        public Email(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Email requerido", nameof(text));

            if (!text.Contains("@") || !text.Contains("."))
                throw new ArgumentException("Email inválido", nameof(text));

            Value = text;
        }

        public override string ToString() => Value;
    }
}
