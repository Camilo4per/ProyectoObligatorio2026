using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using DTOs.DTOs;

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