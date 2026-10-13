using Microsoft.AspNetCore.Mvc;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using DTOs.DTOs;
using Dominio.Excepciones;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IEncontrarTodosUsuarios _encontrarTodosUsuariosCu;
        private IObtenerUsuarioPorId _obtenerUsuarioPorIdCu;
        private IAgregarUsuario _agregarUsuarioCu;
        private ILoginUsuario _loginUsuarioCu;
        public UsuarioController(
            IEncontrarTodosUsuarios encontrarTodosUsuarios, 
            IObtenerUsuarioPorId obtenerUsuarioPorId,
            IAgregarUsuario agregarUsuario,
            ILoginUsuario loginUsuario
            )
        {
            _encontrarTodosUsuariosCu = encontrarTodosUsuarios;
            _obtenerUsuarioPorIdCu = obtenerUsuarioPorId;
            _agregarUsuarioCu = agregarUsuario;
            _loginUsuarioCu = loginUsuario;
        }

        // GET: api/<UsuarioController>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<IEnumerable<UsuarioDTO>> Get()
        {
            try
            {
                return Ok(_encontrarTodosUsuariosCu.Ejecutar());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult<IEnumerable<UsuarioDTO>> Get(int id)
        {
            try
            {
                return Ok(_obtenerUsuarioPorIdCu.Ejecutar(id));
            }
            catch (UsuarioException uex)
            { 
                return BadRequest(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /*
        // POST api/<UsuarioController>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Post([FromBody] UsuarioDTO dto)
        {
            try
            {
                _agregarUsuarioCu.Ejecutar(dto);
                return Created("api/Usuario", dto);
            }
            catch (UsuarioException uex)
            {
                return BadRequest(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        */

        // POST api/<UsuarioController>/Login
        [HttpPost("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Login([FromBody] UsuarioDTO dto)
        {
            try
            {
                return Ok(_loginUsuarioCu.Ejecutar(dto.NombreUsuario, dto.Password));
            }
            catch (UsuarioException uex)
            {
                return BadRequest(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
