using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using System.Collections.Generic;
using System.Linq;

namespace LogicaAplicacion.CasosDeUso.Historias
{
    public class ModificarHistoriaCU : IModificarHistoria
    {
        private IRepositorioHistorias repositorioHistorias;
        private IRepositorioCategorias repositorioCategorias;
        private IRegistrarAuditoria _registrarAuditoria;

        public ModificarHistoriaCU(
            IRepositorioHistorias repoHistorias,
            IRepositorioCategorias repoCategorias,
            IRegistrarAuditoria registrarAuditoria)
        {
            repositorioHistorias = repoHistorias;
            repositorioCategorias = repoCategorias;
            _registrarAuditoria = registrarAuditoria;
        }

        public void Ejecutar(HistoriaDTO dto, int? idUsuarioAdmin = null)
        {
            if (dto == null)
                throw new HistoriaException("Los datos de la historia no pueden ser nulos.");
            if (dto.Estado != (int)EstadoHistoria.Borrador && dto.Estado != (int)EstadoHistoria.Publicada)
                throw new HistoriaException("El estado de la historia es inválido.");

            HistoriaDTO completo = HistoriaMapper.ToDTO(repositorioHistorias.FindById(dto.Id));
            int estadoAnterior = completo.Estado;

            // Una historia publicada es de sólo lectura: se permite Publicada -> Borrador
            // (despublicar), pero no cambiar el contenido mientras siga publicada.
            bool siguePublicada = estadoAnterior == (int)EstadoHistoria.Publicada
                && dto.Estado == (int)EstadoHistoria.Publicada;

            if (siguePublicada && HayCambiosDeContenido(dto, completo))
            {
                throw new HistoriaException(
                    "Una historia publicada no se puede modificar. Vuelva a ponerla en borrador para editarla.");
            }

            if (dto.CapituloInicial != null && completo.CapituloInicial != null)
            {
                bool yaTieneOtrosCapitulos = completo.Capitulos != null && completo.Capitulos.Count > 1;
                bool cambioTitulo = !string.Equals(dto.CapituloInicial.Titulo, completo.CapituloInicial.Titulo);
                bool cambioTexto = !string.Equals(dto.CapituloInicial.Texto, completo.CapituloInicial.Texto);

                if (yaTieneOtrosCapitulos && (cambioTitulo || cambioTexto))
                {
                    throw new HistoriaException(
                        "El capítulo inicial no se puede modificar porque la historia ya posee otros capítulos agregados.");
                }

                if (!yaTieneOtrosCapitulos)
                {
                    completo.CapituloInicial.Titulo = dto.CapituloInicial.Titulo;
                    completo.CapituloInicial.Texto = dto.CapituloInicial.Texto;
                    if (completo.Capitulos != null && completo.Capitulos.Count > 0)
                    {
                        completo.Capitulos[0].Titulo = dto.CapituloInicial.Titulo;
                        completo.Capitulos[0].Texto = dto.CapituloInicial.Texto;
                    }
                }
            }

            completo.Titulo = dto.Titulo;
            completo.Sinopsis = dto.Sinopsis;
            completo.Estado = dto.Estado;
            completo.CategoriaIds = dto.CategoriaIds;
            completo.Categorias = ResolverCategorias(dto.CategoriaIds);

            repositorioHistorias.Update(HistoriaMapper.FromDTO(completo));

            if (idUsuarioAdmin.HasValue && idUsuarioAdmin.Value > 0)
            {
                string accion = (dto.Estado == (int)EstadoHistoria.Publicada && estadoAnterior != (int)EstadoHistoria.Publicada)
                    ? "Publicación de Historia"
                    : "Modificación de Historia";

                _registrarAuditoria.Ejecutar(accion, dto.Id, idUsuarioAdmin.Value);
            }
        }

        private bool HayCambiosDeContenido(HistoriaDTO dto, HistoriaDTO actual)
        {
            if (!string.Equals(dto.Titulo, actual.Titulo)) return true;
            if (!string.Equals(dto.Sinopsis, actual.Sinopsis)) return true;

            if (dto.CapituloInicial != null && actual.CapituloInicial != null)
            {
                if (!string.Equals(dto.CapituloInicial.Titulo, actual.CapituloInicial.Titulo)) return true;
                if (!string.Equals(dto.CapituloInicial.Texto, actual.CapituloInicial.Texto)) return true;
            }

            List<int> idsActuales = actual.Categorias?.Select(c => c.Id).OrderBy(id => id).ToList() ?? new List<int>();
            List<int> idsNuevos = dto.CategoriaIds?.OrderBy(id => id).ToList() ?? new List<int>();

            return !idsActuales.SequenceEqual(idsNuevos);
        }

        private List<CategoriaDTO> ResolverCategorias(List<int> ids)
        {
            List<CategoriaDTO> resultado = new List<CategoriaDTO>();
            if (ids != null)
            {
                foreach (int id in ids)
                {
                    resultado.Add(CategoriaMapper.ToDTO(repositorioCategorias.FindById(id)));
                }
            }
            return resultado;
        }
    }
}