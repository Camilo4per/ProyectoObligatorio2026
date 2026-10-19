using Dominio.Entidades;
using Dominio.InterfaceDeRepositorio;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using System;

namespace LogicaAplicacion.CasosDeUso.Auditorias
{
    public class RegistrarAuditoriaCU : IRegistrarAuditoria
    {
        private IRepositorioAuditorias _repoAuditorias;

        public RegistrarAuditoriaCU(IRepositorioAuditorias repoAuditorias)
        {
            _repoAuditorias = repoAuditorias;
        }

        public void Ejecutar(string accion, int idHistoria, int idUsuarioAdmin)
        {
            Auditoria nuevaAuditoria = new Auditoria
            {
                Fecha = DateTime.Now,
                Accion = accion,
                idHistoria = idHistoria,
                idUsuario = idUsuarioAdmin
            };

            _repoAuditorias.Add(nuevaAuditoria);
        }
    }
}
