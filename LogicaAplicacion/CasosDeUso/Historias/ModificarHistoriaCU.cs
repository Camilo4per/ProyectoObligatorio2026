using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using System.Collections.Generic;

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
            HistoriaDTO completo = HistoriaMapper.ToDTO(repositorioHistorias.FindById(dto.Id));
            int estadoAnterior = completo.Estado;

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