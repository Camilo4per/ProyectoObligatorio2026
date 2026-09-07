using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;

namespace Dominio.ValueObjects.Categoria
{
    public sealed record Nombre : IValidable
    {
        public string Valor { get; init; }

        public Nombre(string nombre)
        {
            Valor = nombre;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Valor))
                throw new CategoriaException("Nombre requerido");

            if (Valor.Length < 3)
                throw new CategoriaException("Nombre debe tener al menos 3 caracteres");

            if (Valor.Length > 30)
                throw new CategoriaException("Nombre no puede exceder los 30 caracteres");
        }

        public override string ToString() => Valor;
    }
}