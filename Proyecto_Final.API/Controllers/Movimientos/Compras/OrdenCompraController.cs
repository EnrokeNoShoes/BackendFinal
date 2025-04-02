using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;

namespace Proyecto_Final.Controller{

    [ApiController]
    [Route("api/ordencompra")]

    public class OrdenCompraController : ControllerBase
    {
        Dordencompra funcion = new Dordencompra();
        [HttpGet("{id}")]
        public async Task<ActionResult<Mordencompra>> Get(int id)
        {
            
            var presupuestocompra = await funcion.ObtenerOrdenCompraPorId(id);

            if (presupuestocompra == null) 
            {
                return NotFound(); 
            }
            return Ok(presupuestocompra); 
        }
        
        [HttpPost]
        [Route("nuevo")]
        public async Task<ActionResult> PostOrdenCompra([FromBody] Mordencompra ordencompra)
        {
            int resultado = await funcion.InsertarRegistro(ordencompra);

            if (resultado > 0)
            {
                return Ok(new { message = "Orden de compra creado correctamente." });
            }
            else
            {
                return BadRequest(new { message = "No se pudo crear la orden de compra." });
            }
        }
          
        [HttpPut]
        [Route("anular")]
        public async Task<ActionResult> PutActualizarEstado([FromBody] Mordencompra ordencompra)
        {
            try{
                int filasAfectadas = await funcion.ActualizarEstado(ordencompra.codorden, ordencompra.codestado);

                if (filasAfectadas > 0)
                {
                    return Ok(new { message = "Estado de la orden actualizado correctamente." });
                }
                    else
                {
                    return NotFound(new { message = "No se encontró la orden con el código especificado." });
                }
            }catch (Exception ex){
                return BadRequest(new { Error = ex.Message });
            }
            
        }
    }

}