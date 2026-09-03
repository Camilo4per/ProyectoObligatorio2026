using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Capitulo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public abstract class Capitulo : IValidable
    {
        public int Id { get; }

        // Value Objects
        public Titulo Titulo { get; set; }
        public Texto Texto { get; set; }

        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
