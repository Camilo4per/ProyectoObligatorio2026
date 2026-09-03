using Dominio.InterfacesDelDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Opcion : IValidable
    {
        public string Texto { get; set; }
        public Capitulo CapDestino { get; set; }
        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
