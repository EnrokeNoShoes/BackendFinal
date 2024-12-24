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
    }
}
