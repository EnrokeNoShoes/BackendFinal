using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Shared.Compras.DTO;
using Proyecto_Final.Services;
using Proyecto_Final.Shared.Compras;

namespace Proyecto_Final.Controller
{
    [ApiController]
    [Route("api/pedidocompra")]
    public class PedidoCompraController : ControllerBase
    {
        private readonly IPedidoCompraService _pedidoService;

        public PedidoCompraController(IPedidoCompraService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoCompraDto>> Get(int id)
        {
            var pedido = await _pedidoService.GetPedidoCompraByIdAsync(id);

            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<PedidoCompraDto>>> GetList()
        {
            var pedidos = await _pedidoService.GetAllPedidosCompraAsync();

            if (pedidos == null || pedidos.Count == 0)
                return NotFound();

            return Ok(pedidos);
        }

        
        [HttpPost("nuevo")]
        public async Task<ActionResult> Post([FromBody] PedidoCompra pedido)
        {
            try
            {
                
                var pedidoCompra = new PedidoCompra
                {
                    codpedidocompra = pedido.codpedidocompra,
                    codcomprobante = pedido.codcomprobante,
                    numpedidocompra =pedido.numpedidocompra,
                    fechapedido = pedido.fechapedido,
                    codestado = pedido.codestado,
                    codsucursal = pedido.codsucursal,
                    codusu = pedido.codusu,
                    detalles = pedido.detalles.Select(d => new PedidoCompraDetalle
                    {
                        codproducto = d.codproducto,
                        cantidad = d.cantidad,
                        costoulitmo = d.costoulitmo
                    }).ToList()
                };

                var codGenerado = await _pedidoService.InsertarPedidoCompra(pedidoCompra);

                return Ok(new
                {
                    message = "Pedido creado correctamente.",
                    id = codGenerado
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el pedido: {ex.Message}" });
            }
        }

        [HttpPut("anular")]
        public async Task<ActionResult> Anular([FromQuery] int codpedidocompra, [FromQuery] int codestado)
        {
            try
            {
                var filasAfectadas = await _pedidoService.ActualizarEstadoPedidoCompra(codpedidocompra, codestado);

                if (filasAfectadas > 0)
                    return Ok(new { message = "Estado del pedido actualizado correctamente." });

                return NotFound(new { message = "Pedido no encontrado o sin cambios." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"MSG: {ex.Message}" });
            }
        }
    }
}
