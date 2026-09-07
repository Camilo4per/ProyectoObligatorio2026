using Dominio.Entidades;
using Dominio.ValueObjects.Categoria;
using DTOs.DTOs;

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

        public static CategoriaDTO ToDTO(Categoria categoria)
        {
            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre.Valor
            };
        }
    }
}