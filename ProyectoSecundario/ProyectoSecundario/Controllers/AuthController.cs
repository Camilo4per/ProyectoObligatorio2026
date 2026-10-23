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
                TokenModel tokenModel = JsonConvert.DeserializeObject<TokenModel>(objetoJson);
                HttpContext.Session.SetString("token", tokenModel.Token);
                HttpContext.Session.SetString("NombreUsuario", tokenModel.Usuario.NombreUsuario);
                HttpContext.Session.SetString("Rol", tokenModel.Usuario.Rol == 0 ? "Administrador" : "Usuario");
                return RedirectToAction("Index", "Historia"); 
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        // GET: /Auth/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("NombreUsuario");
            return RedirectToAction("Login");
        }
    }
}