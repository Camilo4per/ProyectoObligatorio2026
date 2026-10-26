using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSecundario.Auxiliar;
using ProyectoSecundario.Enum;
using ProyectoSecundario.Models;

namespace ProyectoSecundario.Controllers
{
    public class LecturaController : Controller
    {
        private string baseUrlLectura = "http://localhost:5281/api/Lectura";

        // POST: /Lectura/Comenzar
        // Inicia una lectura nueva desde el capítulo inicial de la historia elegida.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Comenzar(int idHistoria)
        {
            string? token = HttpContext.Session.GetString("token");
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            if (idHistoria <= 0 || !idUsuario.HasValue)
            {
                TempData["Error"] = "No se pudo iniciar la lectura: faltan datos de la historia o del usuario.";
                return RedirectToAction("Index", "Historia");
            }

            IniciarLecturaModel solicitud = new IniciarLecturaModel
            {
                idHistoria = idHistoria,
                idUsuario = idUsuario.Value
            };

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrlLectura,
                VerbosHttp.POST,
                solicitud,
                token
            );
            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo iniciar la lectura.";
                return RedirectToAction("Index", "Historia");
            }

            LecturaIniciadaModel? creada = JsonConvert.DeserializeObject<LecturaIniciadaModel>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta));
            if (creada == null)
            {
                TempData["Error"] = "No se pudo iniciar la lectura.";
                return RedirectToAction("Index", "Historia");
            }

            return RedirectToAction("Leer", new
            {
                idLectura = creada.IdLectura,
                idCapitulo = creada.IdCapituloInicial
            });
        }

        // GET: /Lectura/Leer?idLectura=5&idCapitulo=12
        // Muestra el capítulo actual (o el desenlace si ya es final).
        [HttpGet]
        public IActionResult Leer(int idLectura, int idCapitulo)
        {
            string? token = HttpContext.Session.GetString("token");
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            if (idLectura <= 0 || idCapitulo <= 0 || !idUsuario.HasValue)
            {
                TempData["Error"] = "La lectura solicitada no es válida.";
                return RedirectToAction("Index", "Historia");
            }

            string url = baseUrlLectura + "/" + idLectura + "/Capitulo/" + idCapitulo
                         + "?idUsuario=" + idUsuario.Value;

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                url,
                VerbosHttp.GET,
                token: token
            );
            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo obtener el capítulo de la lectura.";
                return RedirectToAction("Index", "Historia");
            }

            PasoLecturaModel? paso = JsonConvert.DeserializeObject<PasoLecturaModel>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta));
            if (paso == null || paso.Capitulo == null)
            {
                TempData["Error"] = "No se pudo obtener el capítulo de la lectura.";
                return RedirectToAction("Index", "Historia");
            }

            return paso.Capitulo.Tipo == 1 ? View("Final", paso) : View("Capitulo", paso);
        }

        // POST: /Lectura/Avanzar
        // El servidor valida que la opción elegida pertenezca al capítulo de origen y,
        // si el destino es final, registra el desenlace en la lectura.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Avanzar(int idLectura, int idCapituloOrigen, int idCapDestino)
        {
            string? token = HttpContext.Session.GetString("token");
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            if (idLectura <= 0 || idCapituloOrigen <= 0 || idCapDestino <= 0 || !idUsuario.HasValue)
            {
                TempData["Error"] = "La opción elegida no es válida.";
                return RedirectToAction("Index", "Historia");
            }

            AvanzarLecturaModel solicitud = new AvanzarLecturaModel
            {
                idUsuario = idUsuario.Value,
                idCapituloOrigen = idCapituloOrigen,
                idCapDestino = idCapDestino
            };

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                baseUrlLectura + "/" + idLectura + "/Avanzar",
                VerbosHttp.POST,
                solicitud,
                token
            );
            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudo avanzar en la lectura.";
                return RedirectToAction("Index", "Historia");
            }

            PasoLecturaModel? paso = JsonConvert.DeserializeObject<PasoLecturaModel>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta));
            if (paso == null || paso.Capitulo == null)
            {
                TempData["Error"] = "No se pudo avanzar en la lectura.";
                return RedirectToAction("Index", "Historia");
            }

            return RedirectToAction("Leer", new
            {
                idLectura = paso.IdLectura,
                idCapitulo = paso.Capitulo.Id
            });
        }
    }
}
