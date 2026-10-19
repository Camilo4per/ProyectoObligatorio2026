using Dominio.Entidades;
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
                Texto = dto.Texto,
                idCapitulo = dto.idCapitulo,
                idCapDestino = dto.idCapDestino,
                CapDestino = dto.CapDestino != null ? CapituloMapper.FromDTO(dto.CapDestino) : null
            };
        }

        public static OpcionDTO ToDTO(Opcion opcion)
        {
            return new OpcionDTO
            {
                Texto = opcion.Texto,
                idCapitulo = opcion.idCapitulo,
                idCapDestino = opcion.idCapDestino,
                CapDestino = opcion.CapDestino != null ? CapituloMapper.ToDTO(opcion.CapDestino) : null
            };
        }
    }
}