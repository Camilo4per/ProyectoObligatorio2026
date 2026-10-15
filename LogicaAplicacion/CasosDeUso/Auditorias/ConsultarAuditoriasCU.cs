using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using System.Collections.Generic;

namespace LogicaAplicacion.CasosDeUso.Auditorias
{
    public class ConsultarAuditoriasCU : IConsultarAuditorias
    {
        private IRepositorioAuditorias _repoAuditorias;

        public ConsultarAuditoriasCU(IRepositorioAuditorias repoAuditorias)
        {
            _repoAuditorias = repoAuditorias;
        }

        public List<AuditoriaDTO> Ejecutar(int? idHistoria, int? idUsuario)
        {
            List<Auditoria> auditorias = _repoAuditorias.Buscar(idHistoria, idUsuario);
            List<AuditoriaDTO> resultado = new List<AuditoriaDTO>();
            foreach (Auditoria item in auditorias)
            {
                resultado.Add(AuditoriaMapper.ToDTO(item));
            }
            return resultado;
        }
    }
}
