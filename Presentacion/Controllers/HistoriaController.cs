using Microsoft.AspNetCore.Mvc;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using DTOs.DTOs;

namespace Presentacion.Controllers
{
    public class HistoriaController : Controller
    {
        private IAgregarHistoria agregarHistoria;
        public HistoriaController(
            IAgregarHistoria agregarHistoriaCu
            )
        {
            agregarHistoria = agregarHistoriaCu;
        }

        // GET: HistoriaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HistoriaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HistoriaController/Create
        public ActionResult Create()
        {
            // Enviar Lista de Categorias a la vista
            return View();
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
            catch
            {
                return View();
            }
        }

        // GET: HistoriaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HistoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HistoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HistoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
