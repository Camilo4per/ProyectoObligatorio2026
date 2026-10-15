using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Capitulos;
using System.Collections.Generic;
using System.Linq;

namespace LogicaAplicacion.CasosDeUso.Capitulos
{
    public class AgregarCapituloCU : IAgregarCapitulo
    {
        private IRepositorioHistorias _repoHistorias;
        private IRepositorioIntermedios _repoIntermedios;
        private IRepositorioFinales _repoFinales;
        private IRegistrarAuditoria _registrarAuditoria;

        public AgregarCapituloCU(
            IRepositorioHistorias repoHistorias,
            IRepositorioIntermedios repoIntermedios,
            IRepositorioFinales repoFinales,
            IRegistrarAuditoria registrarAuditoria)
        {
            _repoHistorias = repoHistorias;
            _repoIntermedios = repoIntermedios;
            _repoFinales = repoFinales;
            _registrarAuditoria = registrarAuditoria;
        }

        public void Ejecutar(CapituloDTO nuevoCapituloDto, int idCapituloAntecedente, string textoOpcion, int? idUsuarioAdmin = null)
        {
            if (nuevoCapituloDto == null)
            {
                throw new CapituloException("Los datos del nuevo capítulo no pueden ser nulos.");
            }

            if (!nuevoCapituloDto.idHistoria.HasValue)
            {
                throw new CapituloException("Debe especificar la historia a la que pertenece el capítulo.");
            }

            if (string.IsNullOrWhiteSpace(textoOpcion))
            {
                throw new OpcionException("Texto requerido");
            }

            // 1. Obtener historia y verificar su existencia
            Historia historia = _repoHistorias.FindById(nuevoCapituloDto.idHistoria.Value);
            if (historia == null)
            {
                throw new HistoriaException("Historia inexistente");
            }

            // 2. Verificar que el capítulo antecedente exista en la historia y sea intermedio
            Capitulo? capituloAntecedente = historia.Capitulos.FirstOrDefault(c => c.Id == idCapituloAntecedente);
            if (capituloAntecedente == null)
            {
                throw new CapituloException("El capítulo antecedente no pertenece a la historia indicada.");
            }

            if (capituloAntecedente is Final)
            {
                throw new CapituloException("Solamente se podrán agregar capítulos a continuación de capítulos de tipo intermedio (no finales).");
            }

            if (!(capituloAntecedente is Intermedio))
            {
                throw new CapituloException("El capítulo antecedente debe ser de tipo intermedio.");
            }

            // 3. Mapear y validar el nuevo capítulo
            Capitulo nuevoCapitulo = CapituloMapper.FromDTO(nuevoCapituloDto);
            nuevoCapitulo.idHistoria = historia.Id;
            nuevoCapitulo.Validar();

            // 4. Persistir el nuevo capítulo según su tipo
            if (nuevoCapitulo is Intermedio nuevoIntermedio)
            {
                _repoIntermedios.Add(nuevoIntermedio);
            }
            else if (nuevoCapitulo is Final nuevoFinal)
            {
                _repoFinales.Add(nuevoFinal);
            }
            else
            {
                throw new CapituloException("Tipo de capítulo no reconocido.");
            }

            // 5. Cargar el capítulo intermedio antecedente para agregarle la nueva opción
            Intermedio intermedioAntecedente = _repoIntermedios.FindById(idCapituloAntecedente);
            if (intermedioAntecedente.Opciones == null)
            {
                intermedioAntecedente.Opciones = new List<Opcion>();
            }

            Opcion nuevaOpcion = new Opcion
            {
                Texto = textoOpcion,
                idCapitulo = intermedioAntecedente.Id,
                idCapDestino = nuevoCapitulo.Id,
                CapDestino = nuevoCapitulo
            };
            nuevaOpcion.Validar();

            intermedioAntecedente.Opciones.Add(nuevaOpcion);
            _repoIntermedios.Update(intermedioAntecedente);

            // 6. Registro de Auditoría automático
            if (idUsuarioAdmin.HasValue && idUsuarioAdmin.Value > 0)
            {
                _registrarAuditoria.Ejecutar($"Creación de Capítulo: {nuevoCapitulo.Titulo.Valor}", historia.Id, idUsuarioAdmin.Value);
            }
        }
    }
}
