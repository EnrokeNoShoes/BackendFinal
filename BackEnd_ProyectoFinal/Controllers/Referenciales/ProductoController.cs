using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;


namespace ProyectoFinal.Controllers{


    [ApiController]
    [Route("api/producto")]
    public class ProductoController : ControllerBase
    {
        [HttpGet("lista")]
        public async Task<ActionResult<List<Mproducto>>> Get()
        {
            var funcion = new Dproducto();
            var lista = await funcion.GetProductoList();
            return lista;
        } 

        [HttpGet("buscador")]
        public async Task<ActionResult<List<Mproductosucursal>>> Getsearch()
        {
            var funcion = new Dproducto();
            var lista = await funcion.GetProductoBuscador();
            return lista;
        }  
    }
}