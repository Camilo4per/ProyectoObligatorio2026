using Dominio.Entidades;
using Dominio.ValueObjects.Opcion;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class OpcionMapper
    {
        // Aca uso operadores ternarios para manejar el caso en que CapDestino sea un Final o un Intermedio, ya que ambos tienen diferentes estructuras y propiedades.
        public static Opcion FromDTO(OpcionDTO dto)
        {
            return new Opcion
            {
                Id = dto.Id,
                Texto = new TextoOpcion(dto.Texto),
                CapDestino = CapituloMapper.FromDTO(dto.CapDestino)
            };
        }

        public static OpcionDTO ToDTO(Opcion opcion)
        {
            return new OpcionDTO
            {
                Id = opcion.Id,
                Texto = opcion.Texto.Valor,
                CapDestino = CapituloMapper.ToDTO(opcion.CapDestino)
            };
        }
    }
}