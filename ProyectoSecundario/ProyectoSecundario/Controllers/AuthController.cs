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
        private string baseUrl = "http://localhost:5209/api/Auth/login";

        // GET: /Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction("Create", "Categoria"); 
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Usuario");
            return RedirectToAction("Login");
        }
    }
}