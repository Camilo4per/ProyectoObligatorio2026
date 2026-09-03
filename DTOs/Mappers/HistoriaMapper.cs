using Dominio.Entidades;
using Dominio.ValueObjects.Historia;
using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.Mappers
{
    public class HistoriaMapper
    {
        public static Historia FromDTO(HistoriaDTO dto)
        {
            return new Historia
            {
                Id = dto.Id,
                Titulo = new Titulo(dto.Titulo),
                Sinopsis = new Sinopsis(dto.Sinopsis),
                Categorias = dto.Categorias,
                Capitulo = dto.Capitulo,
                Estado = dto.Estado
            };
        }
    }
}