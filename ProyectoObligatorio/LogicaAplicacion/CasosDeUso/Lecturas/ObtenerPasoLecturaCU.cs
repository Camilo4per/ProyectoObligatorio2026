using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ObtenerPasoLecturaCU : IObtenerPasoLectura
    {
        private IRepositorioLecturas _repositorioLecturas;
        private IRepositorioHistorias _repositorioHistorias;

        public ObtenerPasoLecturaCU(
            IRepositorioLecturas repoLecturas,
            IRepositorioHistorias repoHistorias)
        {
            _repositorioLecturas = repoLecturas;
            _repositorioHistorias = repoHistorias;
        }

        public PasoLecturaDTO Ejecutar(int idLectura, int idCapitulo, int idUsuario, bool esAdmin)
        {
            Lectura lectura = _repositorioLecturas.FindById(idLectura);

            if (!esAdmin && lectura.idUsuario != idUsuario)
                throw new LecturaException("La lectura no pertenece al usuario autenticado.");

            if (!lectura.idHistoria.HasValue)
                throw new LecturaException("La lectura no tiene una historia asociada.");

            Historia historia = _repositorioHistorias.FindById(lectura.idHistoria.Value);

            Capitulo capitulo = historia.ObtenerCapitulo(idCapitulo)
                ?? throw new LecturaException("El capítulo no pertenece a la historia de la lectura.");

            return LecturaMapper.ToPasoDTO(lectura, historia, capitulo);
        }
    }
}
