using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class AgregarLecturaCU : IAgregarLectura
    {
        private IRepositorioLecturas _repositorioLecturas;
        private IRepositorioHistorias _repositorioHistorias;
        private IRepositorioUsuarios _repositorioUsuarios;

        public AgregarLecturaCU(
            IRepositorioLecturas repoLecturas,
            IRepositorioHistorias repoHistorias,
            IRepositorioUsuarios repoUsuarios)
        {
            _repositorioLecturas = repoLecturas;
            _repositorioHistorias = repoHistorias;
            _repositorioUsuarios = repoUsuarios;
        }

        public LecturaIniciadaDTO Ejecutar(IniciarLecturaDTO dto)
        {
            // La validación de los datos de entrada es responsabilidad del caso de uso:
            // el controlador sólo traduce HTTP y delega.
            if (dto == null)
                throw new LecturaException("Los datos de la lectura no pueden ser nulos.");
            if (dto.idHistoria <= 0)
                throw new LecturaException("La historia indicada es inválida.");
            if (dto.idUsuario <= 0)
                throw new LecturaException("El usuario indicado es inválido.");

            int idHistoria = dto.idHistoria;
            int idUsuario = dto.idUsuario;

            // Se cargan las entidades YA trackeadas por el contexto: construir el grafo
            // desde el DTO haría que EF intente insertar un usuario/historia nuevos.
            Historia historia;
            try
            {
                historia = _repositorioHistorias.FindById(idHistoria);
            }
            catch (HistoriaException)
            {
                throw new LecturaException("La historia indicada no existe.");
            }

            if (historia.Estado != EstadoHistoria.Publicada)
                throw new LecturaException("Solo se puede iniciar la lectura de una historia publicada.");

            Usuario usuario;
            try
            {
                usuario = _repositorioUsuarios.FindById(idUsuario);
            }
            catch (UsuarioException)
            {
                throw new LecturaException("El usuario indicado no existe.");
            }

            Lectura nueva = new Lectura
            {
                idUsuario = usuario.Id,
                Usuario = usuario,
                idHistoria = historia.Id,
                Historia = historia,
                FechaInicio = DateTime.Now
            };

            _repositorioLecturas.Add(nueva);
            return LecturaMapper.ToIniciadaDTO(nueva, historia);
        }
    }
}
