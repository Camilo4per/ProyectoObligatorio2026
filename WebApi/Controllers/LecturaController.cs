using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using DTOs.DTOs;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturaController : ControllerBase
    {
        private IEncontrarTodasLasLecturas _encontrarTodasLasLecturasCu;
        private IObtenerLecturasPorId _obtenerLecturasPorIdCu;
        private IAgregarLectura _agregarLecturaCu;
        public LecturaController(
            IEncontrarTodasLasLecturas encontrarTodasLasLecturas,
            IObtenerLecturasPorId obtenerLecturasPorId,
            IAgregarLectura agregarLectura
            )
        {
            _encontrarTodasLasLecturasCu = encontrarTodasLasLecturas;
            _obtenerLecturasPorIdCu = obtenerLecturasPorId;
            _agregarLecturaCu = agregarLectura;
        }

        // GET: api/<LecturaController>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<IEnumerable<LecturaDTO>> Get()
        {
            try
            {
                return Ok(_encontrarTodasLasLecturasCu.Ejecutar());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<LecturaController>/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<LecturaDTO> Get(int id)
        {
            try
            {
                return Ok(_obtenerLecturasPorIdCu.Ejecutar(id));
            }
            catch (LecturaException lex)
            {
                return BadRequest(lex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /*
        // POST api/<LecturaController>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Post([FromBody] LecturaDTO dto)
        {
            try
            {
                _agregarLecturaCu.Ejecutar(dto);
                return Created("api/Lectura", dto);
            }
            catch (LecturaException lex)
            {
                return BadRequest(lex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        */
    }
}
