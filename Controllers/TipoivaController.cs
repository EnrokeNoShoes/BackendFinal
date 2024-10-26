using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Modelo;
using ProyectoFinal.Datos;

namespace ProyectoFinal.Controllers{

    [ApiController]
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
        [HttpGet]
        public async Task<ActionResult<List<Mtipoiva>>> Get()
        {
            var funcion = new Dtipoiva();
            var lista = await funcion.MostrarTipoiva();
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
        }


    }

}