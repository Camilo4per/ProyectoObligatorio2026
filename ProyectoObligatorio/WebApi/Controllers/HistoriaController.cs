using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoriaController : ControllerBase
    {
        private IEncontrarTodasLasHistorias _encontrarTodasLasHistoriasCu;
        private IEncontrarHistoriasPublicadas _encontrarHistoriasPublicadasCu;
        private IObtenerHistoriaPorId _obtenerHistoriaPorIdCu;

        public HistoriaController(
            IEncontrarTodasLasHistorias encontrarTodasLasHistorias,
            IEncontrarHistoriasPublicadas encontrarHistoriasPublicadas,
            IObtenerHistoriaPorId obtenerHistoriaPorId
            )
        {
            _encontrarTodasLasHistoriasCu = encontrarTodasLasHistorias;
            _encontrarHistoriasPublicadasCu = encontrarHistoriasPublicadas;
            _obtenerHistoriaPorIdCu = obtenerHistoriaPorId;
        }


        // GET: api/<HistoriaController>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<IEnumerable<HistoriaDTO>> Get()
        {
            try
            {
                return Ok(_encontrarTodasLasHistoriasCu.Ejecutar());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<HistoriaController>/Catalogo?idCategoria=2
        [HttpGet("Catalogo")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<IEnumerable<HistoriaCatalogoDTO>> GetCatalogo([FromQuery] int? idCategoria)
        {
            try
            {
                return Ok(_encontrarHistoriasPublicadasCu.Ejecutar(idCategoria));
            }
            catch (HistoriaException hex)
            {
                return BadRequest(hex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<HistoriaController>/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<HistoriaDTO> Get(int id)
        {
            try
            {
                return Ok(_obtenerHistoriaPorIdCu.Ejecutar(id));
            }
            catch (HistoriaException hex)
            {
                return BadRequest(hex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
