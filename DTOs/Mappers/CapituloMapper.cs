using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using DTOs.DTOs;

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