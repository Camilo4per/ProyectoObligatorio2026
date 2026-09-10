
using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;
using Microsoft.EntityFrameworkCore;

namespace Dominio.ValueObjects.Usuario
{
    [Owned]
    public sealed record Password : IValidable
    {
        public string Value { get; init; }

        public Password() { }
        public Password(string value)
        {
            Value = value;
            Validar();
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new UsuarioException("Password requerido");

            if (Value.Length < 8)
                throw new UsuarioException("Password debe tener al menos 8 caracteres");

            bool hasLower = false;
            bool hasUpper = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char ch in Value)
            {
                if (char.IsLower(ch)) hasLower = true;
                else if (char.IsUpper(ch)) hasUpper = true;
                else if (char.IsDigit(ch)) hasDigit = true;
                else hasSpecial = true;

                if (hasLower && hasUpper && hasDigit && hasSpecial) break;
            }

            if (!(hasLower && hasUpper && hasDigit && hasSpecial))
            {
                throw new UsuarioException("Password debe contener al menos una minúscula, una mayúscula, un número y un carácter especial");
            }
        }

        // Verifica igualdad simple (sin hash por ahora)
        public bool Verify(string value) => value == Value;

        // No revelar el valor en ToString
        public override string ToString() => "••••••••";

        
    }
}
