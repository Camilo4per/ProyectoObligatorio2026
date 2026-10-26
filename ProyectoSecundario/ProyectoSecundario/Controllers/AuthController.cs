using ProyectoSecundario.Auxiliar;
using ProyectoSecundario.Enum;
using ProyectoSecundario.Models;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using Newtonsoft.Json;

namespace ProyectoSecundario.Controllers
{
    public class AuthController : Controller
    {
        private string baseUrl = "http://localhost:5281/api/Usuario/Login";

        // GET: /Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrl,
                VerbosHttp.POST,
                model
            );
            if (respuesta.IsSuccessStatusCode)
            {
                string objetoJson = ClienteHttpAuxiliar.ObtenerBody(respuesta);
                TokenModel? tokenModel = JsonConvert.DeserializeObject<TokenModel>(objetoJson);
                if (tokenModel?.Usuario == null)
                {
                    ViewBag.Error = "No se pudo obtener la información del usuario";
                    return View();
                }

                HttpContext.Session.SetString("token", tokenModel.Token);
                HttpContext.Session.SetString("NombreUsuario", tokenModel.Usuario.NombreUsuario);
                HttpContext.Session.SetString("Rol", tokenModel.Usuario.Rol == 0 ? "Administrador" : "Usuario");
                HttpContext.Session.SetInt32("IdUsuario", tokenModel.Usuario.Id);
                return RedirectToAction("Index", "Historia"); 
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        // GET: /Auth/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            // Limpia toda la sesión (token, Rol, NombreUsuario e IdUsuario): si sólo se
            // borraba el token, el layout seguía mostrando la sesión como activa.
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}