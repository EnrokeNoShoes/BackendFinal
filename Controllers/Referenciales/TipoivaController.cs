using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Modelo;
using ProyectoFinal.Datos;

namespace ProyectoFinal.Controllers{

    [ApiController]
    /*[Authorize]*/
    [Route("api/tipoiva")]
    public class TipoivaController : ControllerBase
    {
        [HttpPost]
        [Route("insertar")]
        public async Task Post([FromBody] Mtipoiva parametros)
        {
            var funcion = new Dtipoiva();
            await funcion.InsertarTipoiva(parametros);
        }
        [HttpDelete]
        [Route("eliminar")]
        public async Task<ActionResult> Delete([FromBody] Mtipoiva parametros)
        {
            var funcion = new Dtipoiva();
            var resultado = await funcion.Eliminartipoiva(parametros.codiva);
            if (resultado == "Eliminado correctamente")
            {
                return NoContent(); // No se devuelve contenido en caso de éxito
            }
            return NotFound(new { message = resultado }); // Si no se encontró el registro
        }
        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<Mtipoiva>>> Get()
        {
            var funcion = new Dtipoiva();
            var lista = await funcion.MostrarTipoiva();
            return lista;
        }

        /*[HttpGet]
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
        }*/



    }

}