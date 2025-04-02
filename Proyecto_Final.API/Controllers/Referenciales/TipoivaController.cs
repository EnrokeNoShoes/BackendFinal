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
                return NoContent();
            }
            return NotFound(new { message = resultado });
        }
        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<Mtipoiva>>> Get()
        {
            var funcion = new Dtipoiva();
            var lista = await funcion.MostrarTipoiva();
            return lista;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Mtipoiva>>> GetID(int id)
        {
            var funcion = new Dtipoiva();
            var tipoiva = await funcion.MostrarIvaporID(id);
            if (tipoiva == null) 
            {
                return NotFound();
            }
            return Ok(tipoiva);
        }
   
        [HttpPut]
        [Route("modificar")]
        public async Task Put([FromBody] Mtipoiva parametros)
        {
            var funcion = new Dtipoiva();
            await funcion.ModificarTipoIva(parametros.codiva,parametros.desiva, parametros.coheficiente);
        }


    }

}