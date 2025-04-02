using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;


namespace ProyectoFinal.Controllers{


    [ApiController]
    [Route("api/sucursal")]
    public class SucursalController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Msucursal>>> Get()
        {
            var funcion = new Dsucursal();
            var lista = await funcion.MostrarSucursal();
            return lista;
        }   
    }
}