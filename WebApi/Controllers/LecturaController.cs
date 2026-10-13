using LogicaAplicacion.InterfacesDeCasoDeUso.Lecturas;
using DTOs.DTOs;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturaController : ControllerBase
    {
        // GET: api/<LecturaController>
        [HttpGet]
        public ActionResult<IEnumerable<LecturaDTO>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LecturaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LecturaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LecturaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LecturaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
