using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Opcion
    {
        public string Texto { get; set; }
        public Capitulo CapDestino { get; set; }

        public Opcion(string texto, Capitulo capDestino)
        {
            Texto = texto;
            CapDestino = capDestino;
        }
    }
}
