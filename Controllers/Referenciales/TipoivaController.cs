using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Modelo;
using ProyectoFinal.Datos;

namespace ProyectoFinal.Controllers{

   /* [ApiController]
    [Authorize]*/
    [Route("api/tipoiva")]
    public class TipoivaController : ControllerBase
    {
        [HttpPost]
        public async Task Post([FromBody] Mtipoiva parametros)
        {
            var funcion = new Dtipoiva();
            await funcion.InsertarTipoiva(parametros);
        }

         [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var funcion = new Dtipoiva();
            var paramateros = new Mtipoiva();
            paramateros.codiva = id;
            await funcion.Eliminartipoiva(paramateros);
            return NoContent();
        }
        /*[HttpGet]
        public async Task<ActionResult<List<Mtipoiva>>> Get(int codempresa)
        {
            var funcion = new Dtipoiva();
            var lista = await funcion.MostrarTipoiva(codempresa);
            return lista;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mtipoiva>> Get(int id)
        {
            var funcion = new Dtipoiva();
            var tipoiva = await funcion.MostrarIvaporID(id); // Llama al método que obtiene la empresa por ID
            if (tipoiva == null) // Si no se encuentra la empresa
            {
                return NotFound(); // Retorna un 404 Not Found
            }
            return Ok(tipoiva); // Retorna un 200 OK con el objeto de la empresa
        }*/

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] int? codempresa, [FromQuery] int? id)
        {
            var funcion = new Dtipoiva();
            
            if (codempresa.HasValue)
            {
                var lista = await funcion.MostrarTipoiva(codempresa.Value);
                return Ok(lista);
            }
            else if (id.HasValue)
            {
                var tipoiva = await funcion.MostrarIvaporID(id.Value);
                if (tipoiva == null)
                {
                    return NotFound();
                }
                return Ok(tipoiva);
            }
            
            return BadRequest("Debe proporcionar un codempresa o un id.");
        }



    }

}