using Dominio.Entidades;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Auditorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Presentacion.Controllers
{
    public class AuditoriaController : Controller
    {
        private IConsultarAuditorias _consultarAuditoriasCU;
        private IEncontrarTodasHistorias _encontrarTodasHistoriasCU;
        private IEncontrarTodosUsuarios _encontrarTodosUsuariosCU;

        public AuditoriaController(
            IConsultarAuditorias consultarAuditoriasCU,
            IEncontrarTodasHistorias encontrarTodasHistoriasCU,
            IEncontrarTodosUsuarios encontrarTodosUsuariosCU)
        {
            _consultarAuditoriasCU = consultarAuditoriasCU;
            _encontrarTodasHistoriasCU = encontrarTodasHistoriasCU;
            _encontrarTodosUsuariosCU = encontrarTodosUsuariosCU;
        }

        // GET: Auditoria/Index?idHistoria=...&idUsuario=...
        public ActionResult Index(int? idHistoria, int? idUsuario)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            List<AuditoriaDTO> auditorias = _consultarAuditoriasCU.Ejecutar(idHistoria, idUsuario);

            // Cargar historias para el filtro
            List<HistoriaDTO> historias = _encontrarTodasHistoriasCU.Ejecutar();
            ViewBag.Historias = new SelectList(historias, "Id", "Titulo", idHistoria);

            // Cargar usuarios administradores para el filtro
            List<UsuarioDTO> todosUsuarios = _encontrarTodosUsuariosCU.Ejecutar();
            List<UsuarioDTO> administradores = new List<UsuarioDTO>();
            if (todosUsuarios != null)
            {
                foreach (UsuarioDTO u in todosUsuarios)
                {
                    if (u.Rol == (int)RolesUsuario.Administrador)
                    {
                        administradores.Add(u);
                    }
                }
            }
            ViewBag.Usuarios = new SelectList(administradores, "Id", "NombreUsuario", idUsuario);

            ViewBag.IdHistoriaSeleccionada = idHistoria;
            ViewBag.IdUsuarioSeleccionado = idUsuario;

            return View(auditorias);
        }
    }
}
