using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;

namespace DTOs.DTOs
{
    public class OpcionDTO
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public Capitulo CapDestino { get; set; }
    }
}