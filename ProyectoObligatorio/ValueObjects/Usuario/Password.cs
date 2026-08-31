using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ValueObjects.Usuario
{
    public sealed record Password
    {
        public string Value { get; set; }

        public Password(string textoplano)
        {
            if (string.IsNullOrWhiteSpace(textoplano))
                throw new ArgumentException("Password requerido", nameof(textoplano));

            if (textoplano.Length < 8)
                throw new ArgumentException("Password debe tener al menos 8 caracteres", nameof(textoplano));

            bool hasLower = false;
            bool hasUpper = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char ch in textoplano)
            {
                if (char.IsLower(ch)) hasLower = true;
                else if (char.IsUpper(ch)) hasUpper = true;
                else if (char.IsDigit(ch)) hasDigit = true;
                else hasSpecial = true;

                if (hasLower && hasUpper && hasDigit && hasSpecial) break;
            }

            if (!(hasLower && hasUpper && hasDigit && hasSpecial))
                throw new ArgumentException("Password debe contener al menos una minúscula, una mayúscula, un número y un carácter especial", nameof(textoplano));

            Value = textoplano;
        }

        // Verifica igualdad simple (sin hash por ahora)
        public bool Verify(string textoplano) => Value == textoplano;

        // No revelar el valor en ToString
        public override string ToString() => "••••••••";
    }
}
