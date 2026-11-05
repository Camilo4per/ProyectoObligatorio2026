using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;

namespace LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ActualizarLecturaCU : IActualizarLectura
    {
        private IRepositorioLecturas _repositorioLecturas;
        private IRepositorioHistorias _repositorioHistorias;
        private IRepositorioFinales _repositorioFinales;

        public ActualizarLecturaCU(
            IRepositorioLecturas repoLecturas,
            IRepositorioHistorias repoHistorias,
            IRepositorioFinales repoFinales)
        {
            _repositorioLecturas = repoLecturas;
            _repositorioHistorias = repoHistorias;
            _repositorioFinales = repoFinales;
        }

        public void Ejecutar(int id, LecturaDTO dto)
        {
            if (dto == null)
                throw new LecturaException("Los datos de la lectura no pueden ser nulos.");
            if (id <= 0)
                throw new LecturaException("El identificador de la lectura es inválido.");
            if (dto.Id != 0 && dto.Id != id)
                throw new LecturaException("El id del DTO no coincide con el id de la lectura a modificar.");

            // 1. La lectura debe existir (FindById lanza LecturaException si no).
            Lectura existente = _repositorioLecturas.FindById(id);

            // 2. Regla de dominio: una lectura ya finalizada es inmutable.
            existente.ValidarModificacion();

            // 3. Solo el dueño de la lectura puede modificarla.
            if (!dto.idUsuario.HasValue || existente.idUsuario != dto.idUsuario.Value)
                throw new LecturaException("La lectura no pertenece al usuario autenticado.");

            if (!existente.idHistoria.HasValue)
                throw new LecturaException("La lectura no tiene una historia asociada.");
            int idHistoriaExistente = existente.idHistoria.Value;

            // 4. Final alcanzado: debe existir y ser un final de la MISMA historia.
            Final? finalAlcanzado = null;
            if (dto.idFinal.HasValue)
            {
                try
                {
                    finalAlcanzado = _repositorioFinales.FindById(dto.idFinal.Value);
                }
                catch (FinalException)
                {
                    throw new LecturaException("El final alcanzado indicado no existe.");
                }

                if (finalAlcanzado.idHistoria != idHistoriaExistente)
                    throw new LecturaException("El final alcanzado no pertenece a la historia de la lectura.");
            }

            // 5. Armo la entidad con las referencias YA trackeadas de los repositorios.
            //    No uso LecturaMapper.FromDTO(dto) porque crearia un grafo nuevo de
            //    Usuario e Historia y EF intentaria volver a guardarlo.
            Historia historia;
            try
            {
                historia = _repositorioHistorias.FindById(idHistoriaExistente);
            }
            catch (HistoriaException)
            {
                throw new LecturaException("La historia de la lectura no existe.");
            }

            Lectura lecturaActualizada = new Lectura
            {
                Id = id,
                idUsuario = existente.idUsuario,
                Usuario = existente.Usuario,
                idHistoria = existente.idHistoria,
                Historia = historia,
                FechaInicio = existente.FechaInicio,
                FechaFin = dto.FechaFin,
                idFinal = finalAlcanzado?.Id,
                FinalAlcanzado = finalAlcanzado
            };

            _repositorioLecturas.Update(lecturaActualizada);
        }
    }
}
