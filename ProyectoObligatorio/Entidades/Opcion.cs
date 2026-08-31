using ProyectoObligatorio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicaAplicacion.Entidades
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
