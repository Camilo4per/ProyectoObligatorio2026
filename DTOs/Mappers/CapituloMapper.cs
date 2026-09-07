using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using Dominio.ValueObjects.Final;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class CapituloMapper
    {
        public static Intermedio FromIntermedioDTO(CapituloDTO dto)
        {
            return new Intermedio
            {
                Id = dto.Id,
                Titulo = new Titulo(dto.Titulo),
                Texto = new Texto(dto.Texto),
                Opciones = dto.Opciones.Select(OpcionMapper.FromToIntermedioDTO).ToList()
            };
        }

        public static Final FromFinalDTO(CapituloDTO dto)
        {
            return new Final
            {
                Id = dto.Id,
                Titulo = new Titulo(dto.Titulo),
                Texto = new Texto(dto.Texto),
                CantLlegaron = new CantLlegaron(dto.CantLlegaron)
            };
        }


    }
}