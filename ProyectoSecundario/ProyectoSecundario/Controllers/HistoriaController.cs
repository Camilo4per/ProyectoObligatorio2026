using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSecundario.Auxiliar;
using ProyectoSecundario.Enum;
using ProyectoSecundario.Models;

namespace ProyectoSecundario.Controllers
{
    public class HistoriaController : Controller
    {
        private string baseUrlCatalogo = "http://localhost:5281/api/Historia/Catalogo";
        private string baseUrlCategorias = "http://localhost:5281/api/Categoria";
        private string baseUrlRanking = "http://localhost:5281/api/Historia/Ranking";

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
                baseUrlRanking,
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
                baseUrlCategorias,
                VerbosHttp.GET,
                token: token
            );
            if (respuestaCategorias.IsSuccessStatusCode)
            {
                catalogo.Categorias = JsonConvert.DeserializeObject<List<CategoriaModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuestaCategorias)) ?? new List<CategoriaModel>();
            }

            string urlCatalogo = idCategoria.HasValue && idCategoria.Value > 0
                ? baseUrlCatalogo + "?idCategoria=" + idCategoria.Value
                : baseUrlCatalogo;

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
