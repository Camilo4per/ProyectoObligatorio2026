using Dominio.Entidades;
using Dominio.ValueObjects.Categoria;
using DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.Mappers
{
    public class CategoriaMapper
    {
        public static Categoria FromDTO(CategoriaDTO dto)
        {
            return new Categoria
            {
                Id = dto.Id,
                Nombre = new Nombre(dto.Nombre)
            };
        }
    }
}