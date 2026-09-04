using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    public class CapituloController : Controller
    {
        // GET: CapituloController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CapituloController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CapituloController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CapituloController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: CapituloController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CapituloController/Edit/5
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

        // GET: CapituloController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CapituloController/Delete/5
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
