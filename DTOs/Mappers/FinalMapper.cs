using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using Dominio.ValueObjects.Final;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class FinalMapper
    {
        public static Final FromDTO(FinalDTO dto)
        {
            return new Final
            {
                Titulo = new Titulo(dto.Titulo),
                Texto = new Texto(dto.Texto),
                CantLlegaron = new CantLlegaron(dto.CantLlegaron)
            };
        }
    }
}