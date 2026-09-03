using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.Mappers
{
    public class CapituloMapper
    {
        public static Capitulo FromDTO(CapituloDTO dto)
        {
            return new Intermedio
            {
                Titulo = new Titulo(dto.Titulo),
                Texto = new Texto(dto.Texto)
            };
        }
    }
}