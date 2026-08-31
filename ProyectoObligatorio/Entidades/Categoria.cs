using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Categoria
    {
        public int Id { get; }
        public string Nombre { get; set; }

        public Categoria(string nombre)
        {
            Nombre = nombre;
        }
    }
}
