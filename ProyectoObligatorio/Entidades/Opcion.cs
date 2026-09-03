using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Capitulo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Opcion : IValidable
    {
        
        public Capitulo CapDestino { get; set; }

        // Value Objects
        public Texto Texto { get; set; }
        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
