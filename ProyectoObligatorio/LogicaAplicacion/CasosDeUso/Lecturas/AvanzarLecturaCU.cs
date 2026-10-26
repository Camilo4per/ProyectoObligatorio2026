using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class AvanzarLecturaCU : IAvanzarLectura
    {
        private IRepositorioLecturas _repositorioLecturas;
        private IRepositorioHistorias _repositorioHistorias;
        private IRepositorioFinales _repositorioFinales;

        public AvanzarLecturaCU(
            IRepositorioLecturas repoLecturas,
            IRepositorioHistorias repoHistorias,
            IRepositorioFinales repoFinales)
        {
            _repositorioLecturas = repoLecturas;
            _repositorioHistorias = repoHistorias;
            _repositorioFinales = repoFinales;
        }

        public PasoLecturaDTO Ejecutar(int idLectura, AvanzarLecturaDTO dto, bool esAdmin)
        {
            if (dto == null)
                throw new LecturaException("Los datos del avance no pueden ser nulos.");
            if (dto.idUsuario <= 0 || dto.idCapituloOrigen <= 0 || dto.idCapDestino <= 0)
                throw new LecturaException("Debe indicar el usuario, el capítulo de origen y el destino elegido.");

            Lectura lectura = _repositorioLecturas.FindById(idLectura);

            if (!esAdmin && lectura.idUsuario != dto.idUsuario)
                throw new LecturaException("La lectura no pertenece al usuario autenticado.");

            // Una lectura finalizada es inmutable: para otro camino hay que iniciar una nueva.
            lectura.ValidarModificacion();

            if (!lectura.idHistoria.HasValue)
                throw new LecturaException("La lectura no tiene una historia asociada.");

            Historia historia = _repositorioHistorias.FindById(lectura.idHistoria.Value);
            if (historia.Estado != EstadoHistoria.Publicada)
                throw new LecturaException("La historia ya no está publicada.");

            Capitulo destino = historia.ObtenerCapitulo(dto.idCapDestino)
                ?? throw new LecturaException("El capítulo destino no pertenece a la historia.");

            if (!historia.TieneOpcion(dto.idCapituloOrigen, dto.idCapDestino))
                throw new LecturaException("La opción elegida no conduce al capítulo indicado.");

            if (destino is Final final)
            {
                lectura.RegistrarFinal(final);
                _repositorioLecturas.Update(lectura);

                final.RegistrarLlegada();
                _repositorioFinales.Update(final);
            }

            return LecturaMapper.ToPasoDTO(lectura, historia, destino);
        }
    }
}
