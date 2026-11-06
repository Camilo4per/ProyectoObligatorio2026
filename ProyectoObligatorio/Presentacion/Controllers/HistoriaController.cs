using Dominio.Entidades;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Auxiliares;

namespace Presentacion.Controllers
{
    public class HistoriaController : Controller
    {
        private IAgregarHistoria agregarHistoria;
        private IEncontrarTodasLasHistorias encontrarTodasLasHistorias;
        private IObtenerHistoriaPorId obtenerHistoriaPorId;
        private IModificarHistoria modificarHistoria;
        private IEliminarHistoria eliminarHistoria;
        private IEncontrarTodasCategorias encontrarTodasCategorias;

        public HistoriaController(
            IAgregarHistoria agregarHistoriaCu,
            IEncontrarTodasLasHistorias encontrarTodasLasHistoriasCu,
            IObtenerHistoriaPorId obtenerHistoriaPorIdCu,
            IModificarHistoria modificarHistoriaCu,
            IEliminarHistoria eliminarHistoriaCu,
            IEncontrarTodasCategorias encontrarTodasCategoriasCu
            )
        {
            agregarHistoria = agregarHistoriaCu;
            encontrarTodasLasHistorias = encontrarTodasLasHistoriasCu;
            obtenerHistoriaPorId = obtenerHistoriaPorIdCu;
            modificarHistoria = modificarHistoriaCu;
            eliminarHistoria = eliminarHistoriaCu;
            encontrarTodasCategorias = encontrarTodasCategoriasCu;
        }


        // GET: HistoriaController
        public ActionResult Index()
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            return View(encontrarTodasLasHistorias.Ejecutar());
        }

        // GET: HistoriaController/Details/5
        public ActionResult Details(int id)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            HistoriaDTO historia = obtenerHistoriaPorId.Ejecutar(id);
            ViewBag.ArbolCapitulos = MetodosAuxiliaresHistoria.ArbolCapitulosAuxiliar(historia);

            return View(historia);
        }

        // GET: HistoriaController/Create
        public ActionResult Create()
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
            return View(new HistoriaDTO
            {
                CapituloInicial = new CapituloDTO(),
                CategoriaIds = new List<int>()
            });
        }

        // POST: HistoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HistoriaDTO dto)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            try
            {
                int? idAdmin = HttpContext.Session.GetInt32("IdUsuario");
                agregarHistoria.Ejecutar(dto, idAdmin);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
                if (dto.CapituloInicial == null) dto.CapituloInicial = new CapituloDTO();
                return View(dto);
            }
        }

        // GET: HistoriaController/Edit/5
        public ActionResult Edit(int id)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            HistoriaDTO dto = obtenerHistoriaPorId.Ejecutar(id);
            dto.CategoriaIds = dto.Categorias?
                .Select(c => c.Id)
                .ToList() ?? new List<int>();

            ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();
            return View(dto);
        }

        // POST: HistoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HistoriaDTO dto)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            try
            {
                dto.Id = id;
                int? idAdmin = HttpContext.Session.GetInt32("IdUsuario");
                modificarHistoria.Ejecutar(dto, idAdmin);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Categorias = encontrarTodasCategorias.Ejecutar();

                HistoriaDTO existente = obtenerHistoriaPorId.Ejecutar(id);
                dto.Capitulos = existente.Capitulos;
                if (dto.CapituloInicial == null) dto.CapituloInicial = existente.CapituloInicial;

                return View(dto);
            }
        }

        // GET: HistoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            return View(obtenerHistoriaPorId.Ejecutar(id));
        }

        // POST: HistoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            try
            {
                eliminarHistoria.Ejecutar(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                // Re-renderiza la confirmación con el model para mostrar el mensaje
                // de error (Delete.cshtml usa @Model).
                return View(obtenerHistoriaPorId.Ejecutar(id));
            }
        }

    }
}