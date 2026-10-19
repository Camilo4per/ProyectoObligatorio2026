using Microsoft.AspNetCore.Mvc;

namespace ProyectoSecundario.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
