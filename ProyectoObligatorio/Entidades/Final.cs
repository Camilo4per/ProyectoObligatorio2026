using Dominio.InterfacesDelDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Final : Capitulo, IValidable
    {
        public int CantLlegaron { get; set; }

        // Falta implementar la validación de Final
        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
