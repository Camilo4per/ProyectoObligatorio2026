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
        private string baseUrl = "http://apiwebm3c.somee.com/api/Usuario/Login";

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
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstilo(string estilo, string? returnUrl)
        {
            // Solo los usuarios autenticados con rol Usuario pueden cambiar el estilo
            if (HttpContext.Session.GetString("Rol") != "Usuario")
            {
                return RedirectToAction("Login", "Auth");
            }

            // Cambia el estilo de lectura en la sesión
            HttpContext.Session.SetString("EstiloLectura", estilo == "Oscuro" ? "Oscuro" : "Claro");

            // Redirige al usuario al lugar donde estaba antes de cambiar el estilo
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Login");
        }
    }
}