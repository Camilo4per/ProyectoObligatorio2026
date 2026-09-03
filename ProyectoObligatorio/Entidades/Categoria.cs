using Dominio.InterfacesDelDominio;
using Dominio.ValueObjects.Categoria;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Categoria : IValidable
    {
        public int Id { get; set; }

        // Value Objects
        public Nombre Nombre { get; set; }

        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
