using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Fina;
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
    }

}