using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Intermedio : Capitulo
    {
        public List<Opcion> Opciones
        {
            get { return new List<Opcion>(Opciones); }
            private set;
        }

        public Intermedio() { }
        public Intermedio(string titulo, string texto, List<Opcion> opciones) : base(titulo, texto)
        {
            Opciones = opciones;
        }
    }
}
