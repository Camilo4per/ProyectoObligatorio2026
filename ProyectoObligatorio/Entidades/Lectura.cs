using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Lectura
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public Historia Historia { get; set; }
        public FechaLectura? FechaLectura { get; set; }
        public Final? FinalAlcanzado { get; set; }
    }
}
