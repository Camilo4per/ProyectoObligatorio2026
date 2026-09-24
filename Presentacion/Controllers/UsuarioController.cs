using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using DTOs.DTOs;
using Dominio.Entidades;

namespace Presentacion.Controllers
{
    public class UsuarioController : Controller
    {
        private IEncontrarTodosUsuarios encontrarTodosUsuarios;
        private IRegistrarUsuario registrarUsuario;
        private IObtenerUsuarioPorId obtenerUsuarioPorId;
        private ILoginUsuario loginUsuario;
        public UsuarioController(
            IRegistrarUsuario registrarUsuarioCu,
            IObtenerUsuarioPorId obtenerUsuarioPorIdCu,
            IEncontrarTodosUsuarios encontrarTodosUsuariosCu,
            ILoginUsuario loginUsuarioCu)
        {
            registrarUsuario = registrarUsuarioCu;
            obtenerUsuarioPorId = obtenerUsuarioPorIdCu;
            encontrarTodosUsuarios = encontrarTodosUsuariosCu;
            loginUsuario = loginUsuarioCu;
        }

        // GET: UsuarioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsuarioController/Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: UsuarioController/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        // POST: UsuarioController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string usuario, string password)
        {
            try
            {
                UsuarioDTO dto = loginUsuario.Ejecutar(usuario, password);
                HttpContext.Session.SetInt32("IdUsuario", dto.Id);
                HttpContext.Session.SetString("Rol", ((RolesUsuario)dto.Rol).ToString());
                HttpContext.Session.SetString("NombreUsuario", dto.NombreUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }


        //===========================================================//
        // CRUD: Registro, Detalles, Modificar, Eliminar
        //===========================================================//

        // GET: UsuarioController/Registro
        public ActionResult Registro()
        {
            return View();
        }

        // POST: UsuarioController/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(UsuarioDTO dto)
        {
            try
            {
                registrarUsuario.Ejecutar(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: UsuarioController/Detalles/5
        public ActionResult Detalles(int id)
        {
            return View(obtenerUsuarioPorId.Ejecutar(id));
        }

        // GET: UsuarioController/Modificar/5
        public ActionResult Modificar(int id)
        {
            return View();
        }

        // POST: UsuarioController/Modificar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modificar(int id, UsuarioDTO dto)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, UsuarioDTO dto)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
