using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSecundario.Auxiliar;
using ProyectoSecundario.Enum;
using ProyectoSecundario.Models;
using System.Buffers.Text;

namespace ProyectoSecundario.Controllers
{
    public class LecturaController : Controller
    {
        private string baseUrl = "http://localhost:5209/api/Lectura";

        public IActionResult Index()
        {
            return View();
        }

        // POST: api/<LecturaController>
        public IActionResult Create(LecturaModel model)
        {
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrl,
                VerbosHttp.POST,
                model
            );
            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Historia");
            }
            ViewBag.Error = "No se pudo crear la lectura";
            return View();
        }

        // PUT: api/<LecturaController>/5
        public IActionResult Update(int id, LecturaModel model)
        {
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrl + "/" + id,
                VerbosHttp.PUT,
                model
            );
            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Historia");
            }

            ViewBag.Error = "No se pudo actualizar la lectura";
            return View();
        }

        // GET api/<LecturaController>/Ranking
        [HttpGet]
        public IActionResult GetRanking()
        {
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrl + "/Ranking",
                VerbosHttp.GET
            );
            if (respuesta.IsSuccessStatusCode)
            {
                string contenido = respuesta.Content.ReadAsStringAsync().Result;
                var ranking = JsonConvert.DeserializeObject<List<LecturaModel>>(contenido);
                return View(ranking);
            }
            ViewBag.Error = "No se pudo obtener el ranking";
            return View();
        }
    }
}
