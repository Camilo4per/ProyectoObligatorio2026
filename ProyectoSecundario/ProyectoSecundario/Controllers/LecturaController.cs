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

        // GET: /Lectura/Historial?mes=10&anio=2026
        [HttpGet]
        public IActionResult Historial(int? mes, int? anio)
        {
            string? token = HttpContext.Session.GetString("token");
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (string.IsNullOrEmpty(token) || !idUsuario.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            int mesSeleccionado = mes ?? DateTime.Now.Month;
            int anioSeleccionado = anio ?? DateTime.Now.Year;

            string url = baseUrlLectura + "/Historial"
                + "?idUsuario=" + idUsuario.Value
                + "&mes=" + mesSeleccionado
                + "&anio=" + anioSeleccionado;

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                url,
                VerbosHttp.GET,
                token: token
            );

            List<HistorialLecturaModel> historial =
                new List<HistorialLecturaModel>();

            if (respuesta.IsSuccessStatusCode)
            {
                historial = JsonConvert.DeserializeObject<
                    List<HistorialLecturaModel>>(
                        ClienteHttpAuxiliar.ObtenerBody(respuesta))
                    ?? new List<HistorialLecturaModel>();
            }
            else
            {
                ViewBag.Error =
                    "No se pudo obtener el historial de lecturas.";
            }

            ViewBag.Mes = mesSeleccionado;
            ViewBag.Anio = anioSeleccionado;

            return View(historial);
        }

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
                ViewBag.Error = "No se pudo iniciar la lectura: faltan datos de la historia o del usuario.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
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
                ViewBag.Error = "No se pudo iniciar la lectura.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
            }

            LecturaIniciadaModel? creada = JsonConvert.DeserializeObject<LecturaIniciadaModel>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta));
            if (creada == null)
            {
                ViewBag.Error = "No se pudo iniciar la lectura.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
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
                ViewBag.Error = "La lectura solicitada no es válida.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
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
                ViewBag.Error = "No se pudo obtener el capítulo de la lectura.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
            }

            PasoLecturaModel? paso = JsonConvert.DeserializeObject<PasoLecturaModel>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta));
            if (paso == null || paso.Capitulo == null)
            {
                ViewBag.Error = "No se pudo obtener el capítulo de la lectura.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
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
                ViewBag.Error = "La opción elegida no es válida.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
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
                ViewBag.Error = "No se pudo avanzar en la lectura.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
            }

            PasoLecturaModel? paso = JsonConvert.DeserializeObject<PasoLecturaModel>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta));
            if (paso == null || paso.Capitulo == null)
            {
                ViewBag.Error = "No se pudo avanzar en la lectura.";
                return RedirectToAction("Index", "Historia", new { error = ViewBag.Error });
            }

            return RedirectToAction("Leer", new
            {
                idLectura = paso.IdLectura,
                idCapitulo = paso.Capitulo.Id
            });
        }
    }
}
