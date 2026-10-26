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

        // GET: /Historia?idCategoria=2
        public IActionResult Index(int? idCategoria)
        {
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
