using Dominio.Excepciones;
using Dominio.InterfacesDelDominio;

namespace Dominio.ValueObjects.Final
{
    public sealed record CantLlegaron : IValidable
    {
        public int Valor { get; init; }

        public CantLlegaron(int valor)
        {
            Valor = valor;
        }

        public void Validar()
        {
            if (Valor < 0)
                throw new FinalException("Cantidad de llegadas no puede ser negativa");
        }

        public override string ToString() => Valor.ToString();
    }
}