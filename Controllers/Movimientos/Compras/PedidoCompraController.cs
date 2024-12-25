using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;

namespace Proyecto_Final.Controller
{
    [ApiController]
    [Route("api/pedidocompra")]
    public class PedidoCompraController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Mpedidocompra>> Get(int id)
        {
            var funcion = new Dpedidocompra();
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
            var funcion = new Dpedidocompra();
            var pedido = await funcion.ObtenerPedidoCompraLista();

            if (pedido == null) 
            {
                return NotFound(); 
            }
            return Ok(pedido); 
        }
        [HttpPost]
        [Route("crear")]
        public async Task<ActionResult> PostPedidoCompra([FromBody] Mpedidocompra pedido)
        {
            var funcion = new Dpedidocompra();
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
        [HttpPut]
        [Route("anular")]
        public async Task<ActionResult> PutActualizarEstado([FromBody] Mpedidocompra pedido)
        {
            var funcion = new Dpedidocompra();
            int filasAfectadas = await funcion.ActualizarEstadoPedidoCompra(pedido.codpedidocompra, pedido.codestado);

            if (filasAfectadas > 0)
            {
                return Ok(new { message = "Estado del pedido actualizado correctamente." });
            }
            else
            {
                return NotFound(new { message = "No se encontró el pedido con el código especificado." });
            }
        }

    }

}
