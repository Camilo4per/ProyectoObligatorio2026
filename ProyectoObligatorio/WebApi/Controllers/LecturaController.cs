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
        private IObtenerHistorialLecturas _obtenerHistorialLecturasCu;
        private IObtenerRankingUsuarios _obtenerRankingUsuariosCu;
        private IObtenerPasoLectura _obtenerPasoLecturaCu;
        private IAvanzarLectura _avanzarLecturaCu;
        public LecturaController(
            IEncontrarTodasLasLecturas encontrarTodasLasLecturas,
            IObtenerLecturasPorId obtenerLecturasPorId,
            IAgregarLectura agregarLectura,
            IObtenerHistorialLecturas obtenerHistorialLecturas,
            IObtenerRankingUsuarios obtenerRankingUsuarios,
            IObtenerPasoLectura obtenerPasoLectura,
            IAvanzarLectura avanzarLectura
            )
        {
            _encontrarTodasLasLecturasCu = encontrarTodasLasLecturas;
            _obtenerLecturasPorIdCu = obtenerLecturasPorId;
            _agregarLecturaCu = agregarLectura;
            _obtenerHistorialLecturasCu = obtenerHistorialLecturas;
            _obtenerRankingUsuariosCu = obtenerRankingUsuarios;
            _obtenerPasoLecturaCu = obtenerPasoLectura;
            _avanzarLecturaCu = avanzarLectura;
        }

        // GET: api/<LecturaController>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
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
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult<LecturaDTO> Get(int id, [FromQuery] int idUsuario)
        {
            try
            {
                return Ok(_obtenerLecturasPorIdCu.Ejecutar(id, idUsuario));
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
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult Post([FromBody] IniciarLecturaDTO dto)
        {
            try
            {
                LecturaIniciadaDTO creada = _agregarLecturaCu.Ejecutar(dto);
                return Created("api/Lectura", creada);
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

        // GET: api/<LecturaController>/RankingUsuarios
        [HttpGet("RankingUsuarios")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult<IEnumerable<UsuarioRankingDTO>> RankingUsuarios()
        {
            try
            {
                return Ok(_obtenerRankingUsuariosCu.Ejecutar());
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

        // GET api/<LecturaController>/Historial?idUsuario=3&mes=10&anio=2026
        [HttpGet("Historial")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult<IEnumerable<HistorialLecturaDTO>> Historial(
            [FromQuery] int idUsuario,
            [FromQuery] int mes,
            [FromQuery] int anio)
        {
            try
            {
                return Ok(_obtenerHistorialLecturasCu.Ejecutar(idUsuario, mes, anio));
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
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult<PasoLecturaDTO> GetCapitulo(int id, int idCapitulo, [FromQuery] int idUsuario)
        {
            try
            {
                return Ok(_obtenerPasoLecturaCu.Ejecutar(id, idCapitulo, idUsuario));
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
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult<PasoLecturaDTO> Avanzar(int id, [FromBody] AvanzarLecturaDTO dto)
        {
            try
            {
                return Ok(_avanzarLecturaCu.Ejecutar(id, dto));
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
