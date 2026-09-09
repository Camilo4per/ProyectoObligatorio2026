using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using DTOs.DTOs;

namespace Presentacion.Controllers
{
    public class UsuarioController : Controller
    {
        private IEncontrarTodosUsuarios encontrarTodosUsuarios;
        private IAgregarUsuario agregarUsuario;
        private IObtenerUsuarioPorId obtenerUsuarioPorId;
        public UsuarioController(
            IAgregarUsuario agregarUsuarioCu,
            IObtenerUsuarioPorId obtenerUsuarioPorIdCu,
            IEncontrarTodosUsuarios encontrarTodosUsuariosCu)
        {
            agregarUsuario = agregarUsuarioCu;
            obtenerUsuarioPorId = obtenerUsuarioPorIdCu;
            encontrarTodosUsuarios = encontrarTodosUsuariosCu;
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

        // POST: UsuarioController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsuarioDTO dto)
        {
            try
            {
                agregarUsuario.Ejecutar(dto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // TODO: Colocar mensaje de error en la vista
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
                agregarUsuario.Ejecutar(dto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // TODO: Colocar mensaje de error en la vista
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
            catch
            {
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
            catch
            {
                return View();
            }
        }
    }
}
