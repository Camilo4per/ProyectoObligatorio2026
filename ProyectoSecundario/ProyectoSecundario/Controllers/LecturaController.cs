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
    }
}
