using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSecundario.Auxiliar;
using ProyectoSecundario.Enum;
using ProyectoSecundario.Models;
using System.Buffers.Text;
using System.Net;

namespace ProyectoSecundario.Controllers
{
    public class LecturaController : Controller
    {
        private string baseUrl = "http://localhost:5281/api/Lectura";

        public IActionResult Index()
        {
            return View();
        }

        // GET: api/<LecturaController>
        public IActionResult Get()
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrl,
                VerbosHttp.GET,
                token
            );
            if (respuesta.IsSuccessStatusCode)
            {
                string contenido = respuesta.Content.ReadAsStringAsync().Result;
                var lecturas = JsonConvert.DeserializeObject<List<LecturaModel>>(contenido);
                return View(lecturas);
            }

            ViewBag.Error = "No se pudieron obtener las lecturas";
            return View();
        }

        // POST: api/<LecturaController>
        public IActionResult Create(LecturaModel model)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrl,
                VerbosHttp.POST,
                model,
                token
            );
            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Historia");
            }

            ViewBag.Error = "No se pudo crear la lectura";
            return View(model);
        }

        // PUT: api/<LecturaController>/5
        public IActionResult Update(int id, LecturaModel model)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrl + "/" + id,
                VerbosHttp.PUT,
                model,
                token
            );
            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Historia");
            }

            ViewBag.Error = "No se pudo actualizar la lectura";
            return View(model);
        }

        
    }
}
