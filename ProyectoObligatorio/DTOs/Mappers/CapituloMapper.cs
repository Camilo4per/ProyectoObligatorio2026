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
            // El tipo lo define el DTO: un Final con Opciones = null sigue siendo Final.
            if (dto.Tipo == (int) TipoCapitulo.Final)
                return new Final
                {
                    Id = dto.Id,
                    Titulo = new TituloCapitulo(dto.Titulo),
                    Texto = new TextoCapitulo(dto.Texto),
                    CantLlegaron = new CantLlegaron(dto.CantLlegaron),
                    idHistoria = dto.idHistoria
                };

            return new Intermedio
            {
                Id = dto.Id,
                Titulo = new TituloCapitulo(dto.Titulo),
                Texto = new TextoCapitulo(dto.Texto),
                idHistoria = dto.idHistoria,
                Opciones = dto.Opciones?.Select(OpcionMapper.FromDTO).ToList()
            };
        }

        public static CapituloDTO ToDTO(Capitulo capitulo)
        {
            if (capitulo is Final final)
                return new CapituloDTO
                {
                    Id = final.Id,
                    Tipo = (int) TipoCapitulo.Final,
                    Titulo = final.Titulo.Valor,
                    Texto = final.Texto.Valor,
                    CantLlegaron = final.CantLlegaron.Valor,
                    idHistoria = final.idHistoria
                };

            Intermedio intermedio = (Intermedio) capitulo;
            return new CapituloDTO
            {
                Id = intermedio.Id,
                Tipo = (int) TipoCapitulo.Intermedio,
                Titulo = intermedio.Titulo.Valor,
                Texto = intermedio.Texto.Valor,
                idHistoria = intermedio.idHistoria,
                // Una lista vacía es "no tiene opciones", no "es final".
                Opciones = (intermedio.Opciones ?? new List<Opcion>())
                    .Select(OpcionMapper.ToDTO)
                    .ToList()
            };
        }

        // Proyección de lectura: sin ids de administración ni cantidades.
        public static CapituloLecturaDTO ToLecturaDTO(Capitulo capitulo)
        {
            if (capitulo is Final final)
            {
                return new CapituloLecturaDTO
                {
                    Id = final.Id,
                    Tipo = (int) TipoCapitulo.Final,
                    Titulo = final.Titulo.Valor,
                    Texto = final.Texto.Valor,
                    Opciones = null
                };
            }

            Intermedio intermedio = (Intermedio) capitulo;
            return new CapituloLecturaDTO
            {
                Id = intermedio.Id,
                Tipo = (int) TipoCapitulo.Intermedio,
                Titulo = intermedio.Titulo.Valor,
                Texto = intermedio.Texto.Valor,
                Opciones = (intermedio.Opciones ?? new List<Opcion>())
                    .Select(o => new OpcionLecturaDTO
                    {
                        Texto = o.Texto,
                        idCapDestino = o.idCapDestino
                    })
                    .ToList()
            };
        }
    }
}
