using Dominio.Entidades;
using DTOs.DTOs;
using LogicaAplicacion.CasosDeUso.Usuarios;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    public class UsuarioController : Controller
    {
        private IEncontrarTodosUsuarios encontrarTodosUsuarios;
        private IAgregarUsuario agregarUsuario;
        private IObtenerUsuarioPorId obtenerUsuarioPorId;
        private ILoginCU loginCU;
        public UsuarioController(
            IAgregarUsuario agregarUsuarioCu,
            IObtenerUsuarioPorId obtenerUsuarioPorIdCu,
            IEncontrarTodosUsuarios encontrarTodosUsuariosCu,
            ILoginCU loginCUCu)
        {
            agregarUsuario = agregarUsuarioCu;
            obtenerUsuarioPorId = obtenerUsuarioPorIdCu;
            encontrarTodosUsuarios = encontrarTodosUsuariosCu;
            loginCU = loginCUCu;
        }

        // GET: UsuarioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsuarioController/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        // GET: UsuarioController/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: UsuarioController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDTO dto)
        {
            try
            {
                UsuarioDTO usuDto = loginCU.Ejecutar(dto);

                if (usuDto.Rol != (int)RolesUsuario.Administrador)
                {
                    ViewBag.Error = "Solo los administradores pueden iniciar sesión.";
                    return View();
                }

                HttpContext.Session.SetInt32("IdUsuario", usuDto.Id);
                HttpContext.Session.SetString("Rol", ((RolesUsuario)usuDto.Rol).ToString());
                HttpContext.Session.SetString("NombreUsuario", usuDto.NombreUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: UsuarioController/Registro
        public ActionResult Registro()
        {
            if (HttpContext.Session.GetString("Rol") !=  RolesUsuario.Administrador.ToString())
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        // POST: UsuarioController/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(UsuarioDTO dto)
        {
            if (HttpContext.Session.GetString("Rol") != RolesUsuario.Administrador.ToString())
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                agregarUsuario.Ejecutar(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(dto);
            }
        }
    }
}
