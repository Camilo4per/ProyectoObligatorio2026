using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Final;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Final : Capitulo, IValidable
    {
        public CantLlegaron CantLlegaron { get; set; }

        // Falta implementar la validación de Final
        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
