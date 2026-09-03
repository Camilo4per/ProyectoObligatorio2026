using Dominio.InterfacesDelDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public abstract class Capitulo : IValidable
    {
        public int Id { get; }
        public string Titulo { get; set; }
        public string Texto { get; set; }

        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
