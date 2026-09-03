using Dominio.InterfacesDelDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Categoria : IValidable
    {
        public int Id { get; }
        public string Nombre { get; set; }

        // Value Objects

        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
