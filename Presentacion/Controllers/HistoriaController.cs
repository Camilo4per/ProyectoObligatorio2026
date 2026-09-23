using Dominio.Entidades;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentacion.Controllers
{
    public class HistoriaController : Controller
    {
        private IAgregarHistoria agregarHistoria;
        private IEncontrarTodasHistorias encontrarTodasHistorias;
        private IObtenerHistoriaPorId obtenerHistoriaPorId;
        private IModificarHistoria modificarHistoria;
        private IEliminarHistoria eliminarHistoria;
        private IEncontrarTodasCategorias encontrarTodasCategorias;

        public HistoriaController(
            IAgregarHistoria agregarHistoriaCu,
            IEncontrarTodasHistorias encontrarTodasHistoriasCu,
            IObtenerHistoriaPorId obtenerHistoriaPorIdCu,
            IModificarHistoria modificarHistoriaCu,
            IEliminarHistoria eliminarHistoriaCu,
            IEncontrarTodasCategorias encontrarTodasCategoriasCu
            )
        {
            agregarHistoria = agregarHistoriaCu;
            encontrarTodasHistorias = encontrarTodasHistoriasCu;
            obtenerHistoriaPorId = obtenerHistoriaPorIdCu;
            modificarHistoria = modificarHistoriaCu;
            eliminarHistoria = eliminarHistoriaCu;
            encontrarTodasCategorias = encontrarTodasCategoriasCu;
        }

        // GET: HistoriaController
        public ActionResult Index()
        {
            return View(encontrarTodasHistorias.Ejecutar());
        }

        // GET: HistoriaController/Details/5
        public ActionResult Details(int id)
        {
            return View(obtenerHistoriaPorId.Ejecutar(id));
        }

        // GET: HistoriaController/Create
        public ActionResult Create()
        {
            ViewBag.Categorias = CategoriasSelectList(null);
            return View(new HistoriaDTO { CapituloInicial = new CapituloDTO() });
        }

        // POST: HistoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HistoriaDTO dto)
        {
            try
            {
                agregarHistoria.Ejecutar(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Categorias = CategoriasSelectList(dto.CategoriaIds);
                if (dto.CapituloInicial == null) dto.CapituloInicial = new CapituloDTO();
                return View(dto);
            }
        }

        // GET: HistoriaController/Edit/5
        public ActionResult Edit(int id)
        {
            HistoriaDTO dto = obtenerHistoriaPorId.Ejecutar(id);
            ViewBag.Categorias = CategoriasSelectList(dto.Categorias.Select(c => c.Id).ToList());
            return View(dto);
        }

        // POST: HistoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HistoriaDTO dto)
        {
            try
            {
                dto.Id = id;
                modificarHistoria.Ejecutar(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Categorias = CategoriasSelectList(dto.CategoriaIds);
                return View(dto);
            }
        }

        // GET: HistoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(obtenerHistoriaPorId.Ejecutar(id));
        }

        // POST: HistoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                eliminarHistoria.Ejecutar(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        private List<SelectListItem> CategoriasSelectList(List<int> seleccionadas)
        {
            return encontrarTodasCategorias.Ejecutar()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre,
                    Selected = seleccionadas != null && seleccionadas.Contains(c.Id)
                })
                .ToList();
        }
    }
}