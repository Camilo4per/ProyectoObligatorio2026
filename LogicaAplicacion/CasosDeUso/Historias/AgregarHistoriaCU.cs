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
    public class AgregarHistoriaCU : IAgregarHistoria
    {
        private IRepositorioHistorias repositorioHistorias;
        private IRepositorioCategorias repositorioCategorias;
        private IRegistrarAuditoria _registrarAuditoria;

        public AgregarHistoriaCU(
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
            dto.Estado = (int) EstadoHistoria.Borrador;

            if (dto.CapituloInicial == null)
                throw new HistoriaException("La historia debe crearse con un capítulo inicial");
            dto.CapituloInicial.Tipo = (int) TipoCapitulo.Intermedio;
            dto.CapituloInicial.Opciones = null;
            dto.Capitulos = new List<CapituloDTO> { dto.CapituloInicial };

            dto.Categorias = ResolverCategorias(dto.CategoriaIds);

            Historia historiaCreada = HistoriaMapper.FromDTO(dto);
            repositorioHistorias.Add(historiaCreada);

            if (idUsuarioAdmin.HasValue && idUsuarioAdmin.Value > 0)
            {
                _registrarAuditoria.Ejecutar("Creación de Historia", historiaCreada.Id, idUsuarioAdmin.Value);
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