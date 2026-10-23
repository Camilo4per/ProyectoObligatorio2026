using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Categorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private IEncontrarTodasCategorias _encontrarTodasCategoriasCu;

        public CategoriaController(IEncontrarTodasCategorias encontrarTodasCategorias)
        {
            _encontrarTodasCategoriasCu = encontrarTodasCategorias;
        }

        // GET: api/<CategoriaController>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            try
            {
                return Ok(_encontrarTodasCategoriasCu.Ejecutar());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
