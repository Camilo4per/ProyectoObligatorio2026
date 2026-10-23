using ApiWeb.TokenHandler;
using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private ILoginCU _loginCu;
        private readonly IConfiguration _configuration;
        public UsuarioController(
            IEncontrarTodosUsuarios encontrarTodosUsuarios, 
            IObtenerUsuarioPorId obtenerUsuarioPorId,
            ILoginCU loginCu,
            IConfiguration configuration
            )
        {
            _encontrarTodosUsuariosCu = encontrarTodosUsuarios;
            _obtenerUsuarioPorIdCu = obtenerUsuarioPorId;
            _loginCu = loginCu;
            _configuration = configuration;
        }

        // GET: api/<UsuarioController>
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
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
        [Authorize(Roles = "Administrador")]
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

        // POST api/<UsuarioController>/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult Login([FromBody] LoginDTO dto)
        {
            try
            {
                var usuarioDto = _loginCu.Ejecutar(dto);
                string token = JWTHandler.GenerarToken(usuarioDto, _configuration);
                return Ok(
                    new
                    {
                        Token = token,
                        Usuario = usuarioDto
                    });
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
