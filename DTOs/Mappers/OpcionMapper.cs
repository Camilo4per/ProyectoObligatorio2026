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
                Texto = new TextoOpcion(dto.Texto),
                CapDestino = dto.CapDestino != null ? CapituloMapper.FromDTO(dto.CapDestino) : null
            };
        }

        public static OpcionDTO ToDTO(Opcion opcion)
        {
            return new OpcionDTO
            {
                Texto = opcion.Texto.Valor,
                CapDestino = opcion.CapDestino != null ? CapituloMapper.ToDTO(opcion.CapDestino) : null
            };
        }
    }
}