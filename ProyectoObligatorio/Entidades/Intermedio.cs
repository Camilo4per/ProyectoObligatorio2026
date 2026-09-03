using Dominio.InterfacesDelDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Intermedio : Capitulo, IValidable
    {
        public List<Opcion> Opciones;
        public void Validar()
        {
            throw new NotImplementedException();
        }
    }
}
