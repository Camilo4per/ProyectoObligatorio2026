using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Final : Capitulo
    {
        public int CantLlegaron { get; set; }
        public Final(string titulo, string texto, int cantLlegaron) : base(titulo, texto)
        {
            CantLlegaron = cantLlegaron;
        }
    }
}
