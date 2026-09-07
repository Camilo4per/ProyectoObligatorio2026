using Dominio.Entidades;
using Dominio.ValueObjects.Capitulo;
using Dominio.ValueObjects.Final;
using DTOs.DTOs;

namespace DTOs.Mappers
{
    public class CapituloMapper
    {
        public static Capitulo FromDTO(CapituloDTO dto)
        {
            return dto.Opciones.Count == 0
                ? new Final
                {
                    Id = dto.Id,
                    Titulo = new Titulo(dto.Titulo),
                    Texto = new TextoCapitulo(dto.Texto),
                    CantLlegaron = new CantLlegaron(dto.CantLlegaron)
                }
                : new Intermedio
                {
                    Id = dto.Id,
                    Titulo = new Titulo(dto.Titulo),
                    Texto = new TextoCapitulo(dto.Texto),
                    Opciones = dto.Opciones.Select(OpcionMapper.FromDTO).ToList()
                };
        }

        public static CapituloDTO ToDTO(Capitulo capitulo)
        {
            if (capitulo is Final final)
                return new CapituloDTO
                {
                    Id = final.Id,
                    Titulo = final.Titulo.Valor,
                    Texto = final.Texto.Valor,
                    CantLlegaron = final.CantLlegaron.Valor,
                    Opciones = new List<OpcionDTO>()
                };

            Intermedio intermedio = (Intermedio) capitulo;
            return new CapituloDTO
            {
                Id = intermedio.Id,
                Titulo = intermedio.Titulo.Valor,
                Texto = intermedio.Texto.Valor,
                Opciones = intermedio.Opciones.Select(OpcionMapper.ToDTO).ToList()
            };
        }
    }
}