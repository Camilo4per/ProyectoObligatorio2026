using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public abstract class Capitulo
    {
        public int Id { get; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        
        public Capitulo() { }
        public Capitulo(string titulo, string texto)
        {
            Titulo = titulo;
            Texto = texto;
        }
    }
}
