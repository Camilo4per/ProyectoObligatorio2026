using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class IntermedioMapper
    {
        public static Intermedio FromDTO(IntermedioDTO dto)
        {
            return new Intermedio
            {
                Titulo = new Titulo(dto.Titulo),
                Texto = new Texto(dto.Texto),
                Opciones = dto.Opciones.Select(OpcionMapper.FromDTO).ToList()
            };
        }
    }
}