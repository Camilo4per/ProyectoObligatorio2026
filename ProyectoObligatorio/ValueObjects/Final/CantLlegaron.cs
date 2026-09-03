using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ValueObjects.Final
{
    public sealed record CantLlegaron
    {
        public int Valor { get; init; }

        public CantLlegaron(int valor)
        {
            if (valor < 0)
                throw new ArgumentException("Cantidad de llegadas no puede ser negativa", nameof(valor));

            Valor = valor;
        }

        public override string ToString() => Valor.ToString();
    }
}