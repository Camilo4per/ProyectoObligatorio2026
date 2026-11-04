using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSecundario.Auxiliar;
using ProyectoSecundario.Enum;
using ProyectoSecundario.Models;

namespace ProyectoSecundario.Controllers
{
    public class HistoriaController : Controller
    {
        private const string baseUrl = "http://localhost:5281/api";

        // GET: /Historia/Finales?idHistoria=5
        [HttpGet]
        public IActionResult Finales(int? idHistoria)
        {
            string? token = HttpContext.Session.GetString("token");
            string? rol = HttpContext.Session.GetString("Rol");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (rol != "Administrador")
            {
                return RedirectToAction(nameof(Index));
            }

            FinalesHistoriaViewModel modelo = new FinalesHistoriaViewModel
            {
                IdHistoriaSeleccionada = idHistoria
            };

            HttpResponseMessage respuestaHistorias =
                ClienteHttpAuxiliar.EnviarSolicitud(
                    $"{baseUrl}/Historia",
                    VerbosHttp.GET,
                    token: token
                );

            if (!respuestaHistorias.IsSuccessStatusCode)
            {
                ViewBag.Error = "No se pudo obtener la lista de historias.";
                return View(modelo);
            }

            modelo.Historias =
                JsonConvert.DeserializeObject<List<HistoriaModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuestaHistorias))
                ?? new List<HistoriaModel>();

            if (idHistoria.HasValue && idHistoria.Value > 0)
            {
                HttpResponseMessage respuesta =
                    ClienteHttpAuxiliar.EnviarSolicitud(
                        $"{baseUrl}/Historia/{idHistoria.Value}/Finales",
                        VerbosHttp.GET,
                        token: token
                    );

                if (!respuesta.IsSuccessStatusCode)
                {
                    ViewBag.Error = "No se pudo obtener los finales de la historia.";
                    return View(modelo);
                }

                modelo.Reporte =
                    JsonConvert.DeserializeObject<FinalesHistoriaModel>(
                        ClienteHttpAuxiliar.ObtenerBody(respuesta));
            }

            return View(modelo);
        }

        // GET: /Historia/Ranking
        public IActionResult Ranking()
        {
            string? token = HttpContext.Session.GetString("token");
            string? rol = HttpContext.Session.GetString("Rol");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (rol != "Administrador")
            {
                return RedirectToAction("Index", "Historia");
            }

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                $"{baseUrl}/Historia/Ranking",
                VerbosHttp.GET,
                token: token
            );

            List<HistoriaRankingModel> ranking =
                new List<HistoriaRankingModel>();

            if (respuesta.IsSuccessStatusCode)
            {
                ranking = JsonConvert.DeserializeObject<List<HistoriaRankingModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuesta))
                    ?? new List<HistoriaRankingModel>();
            }
            else
            {
                ViewBag.Error = "No se pudo obtener el ranking de historias.";
            }

            return View(ranking);
        }

        // GET: /Historia/RankingUsuarios
        public IActionResult RankingUsuarios()
        {
            string? token = HttpContext.Session.GetString("token");
            string? rol = HttpContext.Session.GetString("Rol");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (rol != "Administrador")
            {
                return RedirectToAction(nameof(Index));
            }

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                $"{baseUrl}/Lectura/RankingUsuarios",
                VerbosHttp.GET,
                token: token
            );

            List<UsuarioRankingModel> ranking =
                new List<UsuarioRankingModel>();

            if (respuesta.IsSuccessStatusCode)
            {
                ranking = JsonConvert.DeserializeObject<
                    List<UsuarioRankingModel>>(
                        ClienteHttpAuxiliar.ObtenerBody(respuesta))
                    ?? new List<UsuarioRankingModel>();
            }
            else
            {
                ViewBag.Error = "No se pudo obtener el ranking de usuarios.";
            }

            return View(ranking);
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Historia/Historias?idCategoria=2
        public IActionResult Historias(int? idCategoria, string? error = null)
        {
            if (HttpContext.Session.GetString("Rol") != "Usuario")
            {
                return RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            CatalogoViewModel catalogo = new CatalogoViewModel
            {
                IdCategoriaSeleccionada = idCategoria
            };

            HttpResponseMessage respuestaCategorias = ClienteHttpAuxiliar.EnviarSolicitud(
                $"{baseUrl}/Categoria",
                VerbosHttp.GET,
                token: token
            );
            if (respuestaCategorias.IsSuccessStatusCode)
            {
                catalogo.Categorias = JsonConvert.DeserializeObject<List<CategoriaModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuestaCategorias)) ?? new List<CategoriaModel>();
            }

            string urlCatalogo = idCategoria.HasValue && idCategoria.Value > 0
                ? $"{baseUrl}/Historia/Catalogo?idCategoria={idCategoria.Value}"
                : $"{baseUrl}/Historia/Catalogo";

            HttpResponseMessage respuestaCatalogo = ClienteHttpAuxiliar.EnviarSolicitud(
                urlCatalogo,
                VerbosHttp.GET,
                token: token
            );
            if (respuestaCatalogo.IsSuccessStatusCode)
            {
                catalogo.Historias = JsonConvert.DeserializeObject<List<HistoriaCatalogoModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuestaCatalogo)) ?? new List<HistoriaCatalogoModel>();
            }
            else
            {
                ViewBag.Error = "No se pudo obtener el catálogo de historias.";
            }

            return View(catalogo);
        }
    }
}
