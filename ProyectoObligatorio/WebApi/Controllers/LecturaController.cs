using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using DTOs.DTOs;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LecturaController : ControllerBase
    {
        private IEncontrarTodasLasLecturas _encontrarTodasLasLecturasCu;
        private IObtenerLecturasPorId _obtenerLecturasPorIdCu;
        private IAgregarLectura _agregarLecturaCu;
        private IActualizarLectura _actualizarLecturaCu;
        private IObtenerPasoLectura _obtenerPasoLecturaCu;
        private IAvanzarLectura _avanzarLecturaCu;
        public LecturaController(
            IEncontrarTodasLasLecturas encontrarTodasLasLecturas,
            IObtenerLecturasPorId obtenerLecturasPorId,
            IAgregarLectura agregarLectura,
            IActualizarLectura actualizarLectura,
            IObtenerPasoLectura obtenerPasoLectura,
            IAvanzarLectura avanzarLectura
            )
        {
            _encontrarTodasLasLecturasCu = encontrarTodasLasLecturas;
            _obtenerLecturasPorIdCu = obtenerLecturasPorId;
            _agregarLecturaCu = agregarLectura;
            _actualizarLecturaCu = actualizarLectura;
            _obtenerPasoLecturaCu = obtenerPasoLectura;
            _avanzarLecturaCu = avanzarLectura;
        }

        // GET: api/<LecturaController>
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
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

        // GET api/<LecturaController>/5?idUsuario=3
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<LecturaDTO> Get(int id, [FromQuery] int idUsuario)
        {
            try
            {
                return Ok(_obtenerLecturasPorIdCu.Ejecutar(id, idUsuario, User.IsInRole("Administrador")));
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

        // POST api/<LecturaController>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Post([FromBody] IniciarLecturaDTO dto)
        {
            try
            {
                LecturaIniciadaDTO creada = _agregarLecturaCu.Ejecutar(dto);
                return CreatedAtAction(nameof(Get), new { id = creada.IdLectura, idUsuario = dto.idUsuario }, creada);
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

        // GET api/<LecturaController>/5/Capitulo/12?idUsuario=3
        [HttpGet("{id}/Capitulo/{idCapitulo}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<PasoLecturaDTO> GetCapitulo(int id, int idCapitulo, [FromQuery] int idUsuario)
        {
            try
            {
                return Ok(_obtenerPasoLecturaCu.Ejecutar(id, idCapitulo, idUsuario, User.IsInRole("Administrador")));
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

        // POST api/<LecturaController>/5/Avanzar
        [HttpPost("{id}/Avanzar")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<PasoLecturaDTO> Avanzar(int id, [FromBody] AvanzarLecturaDTO dto)
        {
            try
            {
                return Ok(_avanzarLecturaCu.Ejecutar(id, dto, User.IsInRole("Administrador")));
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

        // PUT api/<LecturaController>/5
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Put(int id, [FromBody] LecturaDTO dto)
        {
            try
            {
                if (dto == null || id != dto.Id)
                {
                    return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la solicitud.");
                }
                _actualizarLecturaCu.Ejecutar(id, dto, User.IsInRole("Administrador"));
                return NoContent();
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
    }
}
