using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;

namespace Proyecto_Final.Controller
{
    [ApiController]
    [Route("api/pedidocompra")]
    public class PedidoCompraController : ControllerBase
    {
        private readonly Dpedidocompra _funcion;

        // Inyección de dependencias del constructor
        public PedidoCompraController(Dpedidocompra funcion)
        {
            _funcion = funcion;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mpedidocompra>> Get(int id)
        {
            var pedido = await _funcion.ObtenerPedidoCompraPorId(id);

            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<Mpedidocompra>>> GetList()  // Debería devolver una lista de pedidos
        {
            var pedidos = await _funcion.ObtenerPedidoCompraLista();

            if (pedidos == null || pedidos.Count == 0)
            {
                return NotFound();
            }
            return Ok(pedidos);
        }

        [HttpPost]
        [Route("nuevo")]
        public async Task<ActionResult> PostPedidoCompra([FromBody] Mpedidocompra pedido)
        {
            int resultado = await _funcion.InsertarPedidoCompra(pedido);

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
                int filasAfectadas = await _funcion.ActualizarEstadoPedidoCompra(codpedidocompra, codestado);

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
