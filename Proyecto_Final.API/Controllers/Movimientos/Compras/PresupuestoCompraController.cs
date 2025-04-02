using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;

namespace Proyecto_Final.Controller{

    [ApiController]
    [Route("api/presupuestocompra")]

    public class PresupuestoCompraController : ControllerBase
    {
        Dpresupuestocompra funcion = new Dpresupuestocompra();
        [HttpGet("{id}")]
        public async Task<ActionResult<Mpresupuestocompra>> Get(int id)
        {
            
            var presupuestocompra = await funcion.ObtenerPresupuestoCompraPorId(id);

            if (presupuestocompra == null) 
            {
                return NotFound(); 
            }
            return Ok(presupuestocompra); 
        }
        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<Mpresupuestocompra>> GetList()
        {
            
            var presupuestocompra = await funcion.ObtenerPresupuestoCompraLista();

            if (presupuestocompra == null) 
            {
                return NotFound(); 
            }
            return Ok(presupuestocompra); 
        }    
        [HttpPost]
        [Route("nuevo")]
        public async Task<ActionResult> PostPresupuestoCompra([FromBody] Mpresupuestocompra presupuesto)
        {
            int resultado = await funcion.InsertarRegistro(presupuesto);

            if (resultado > 0)
            {
                return Ok(new { message = "Presupueso de compra creado correctamente." });
            }
            else
            {
                return BadRequest(new { message = "No se pudo crear el presupuesto de compra." });
            }
        }
          
        [HttpPut]
        [Route("anular")]
        public async Task<ActionResult> PutActualizarEstado([FromBody] Mpresupuestocompra presupuesto)
        {
            try{
                int filasAfectadas = await funcion.ActualizarEstado(presupuesto.codpresupuestocompra, presupuesto.codestado);

                if (filasAfectadas > 0)
                {
                    return Ok(new { message = "Estado del presupuesto actualizado correctamente." });
                }
                    else
                {
                    return NotFound(new { message = "No se encontró el presupuesto con el código especificado." });
                }
            }catch (Exception ex){
                return BadRequest(new { Error = ex.Message });
            }
            
        }
    }

}