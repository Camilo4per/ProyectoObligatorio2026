using Microsoft.AspNetCore.Mvc;
using LogicaAplicacion.InterfacesDeCasoDeUso.Usuarios;
using DTOs.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IEncontrarTodosUsuarios _encontrarTodosUsuarios;
        private IObtenerUsuarioPorId _obtenerUsuarioPorId;
        public UsuarioController(
            IEncontrarTodosUsuarios encontrarTodosUsuarios, 
            IObtenerUsuarioPorId obtenerUsuarioPorId
            )
        {
            _encontrarTodosUsuarios = encontrarTodosUsuarios;
            _obtenerUsuarioPorId = obtenerUsuarioPorId;
        }

        // GET: api/<UsuarioController>
        [HttpGet]
        public IEnumerable<UsuarioDTO> Get()
        {
            return _encontrarTodosUsuarios.Ejecutar();
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public UsuarioDTO Get(int id)
        {
            return _obtenerUsuarioPorId.Ejecutar(id);
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
