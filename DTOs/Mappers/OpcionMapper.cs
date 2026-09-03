using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.Mappers
{
    public class OpcionMapper
    {
        public static Opcion FromDTO(OpcionDTO dto)
        {
            return new Opcion
            {
                Texto = new Texto(dto.Texto),
                CapDestino = CapituloMapper.FromDTO(dto.CapDestino)
            };
        }
    }
}