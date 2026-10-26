using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentacion.Filters
{
    // Filtro de autorización del backoffice: sólo un usuario con rol Administrador
    // en sesión puede ejecutar las acciones del controller donde se aplica.
    public class AdministradorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? rol = context.HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                context.Result = new RedirectToActionResult("Login", "Usuario", null);
            }
        }
    }
}
