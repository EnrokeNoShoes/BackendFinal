using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;

namespace Proyecto_Final.Controller
{
    [ApiController]
    [Route("api/pedidocompra")]
    public class PedidoCompraController : ControllerBase
    {
        Dpedidocompra funcion = new Dpedidocompra();
        [HttpGet("{id}")]
        public async Task<ActionResult<Mpedidocompra>> Get(int id)
        {
            
            var pedido = await funcion.ObtenerPedidoCompraPorId(id);

            if (pedido == null) 
            {
                return NotFound(); 
            }
            return Ok(pedido); 
        }
        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<Mpedidocompra>> GetList()
        {
            
            var pedido = await funcion.ObtenerPedidoCompraLista();

            if (pedido == null) 
            {
                return NotFound(); 
            }
            return Ok(pedido); 
        }
        [HttpPost]
        [Route("nuevo")]
        public async Task<ActionResult> PostPedidoCompra([FromBody] Mpedidocompra pedido)
        {
            int resultado = await funcion.InsertarPedidoCompra(pedido);

            if (resultado > 0)
            {
                return Ok(new { message = "Pedido de compra creado correctamente." });
            }
            else
            {
                return BadRequest(new { message = "No se pudo crear el pedido de compra." });
            }
        }
        [HttpPut("anular")]
        public async Task<ActionResult> PutActualizarEstado([FromQuery] int codpedidocompra, [FromQuery] int codestado)
        {
            try
            {
            
                int filasAfectadas = await funcion.ActualizarEstadoPedidoCompra(codpedidocompra, codestado);

                if (filasAfectadas > 0)
                {
                    return Ok(new { message = "Estado del pedido actualizado correctamente." });
                }
                else
                {
                    return NotFound(new { message = "No se encontró el pedido con el código especificado." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }

}
