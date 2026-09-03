using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;

namespace DTOs.DTOs
{
    public class IntermedioDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public List<Opcion> Opciones { get; set; }
    }
}