using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class OpcionMapper
    {
        public static Opcion FromToIntermedioDTO(OpcionDTO dto)
        {
            return new Opcion
            {
                Texto = new Texto(dto.Texto),
                CapDestino = CapituloMapper.FromIntermedioDTO(dto.CapDestino)
            };
        }

        public static Opcion FromToFinalDTO(OpcionDTO dto)
        {
            return new Opcion
            {
                Texto = new Texto(dto.Texto),
                CapDestino = CapituloMapper.FromFinalDTO(dto.CapDestino)
            };
        }
    }
}