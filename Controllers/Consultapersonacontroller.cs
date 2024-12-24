using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;

namespace Proyecto_Final.Controller
{
    [ApiController]
    [Route("api/consultapersona")]
    public class Consultapersonacontroller : ControllerBase
    {
    
    
        [HttpGet("{numdoc}")]
        public async Task<ActionResult<Mconsultapersona>> Get(string numdoc)
        {
            var funcion = new Dconsultapersona();
            var empresa = await funcion.Mostrarpersona(numdoc); // Llama al método que obtiene la empresa por ID
            if (empresa == null) // Si no se encuentra la empresa
            {
                return NotFound(); // Retorna un 404 Not Found
            }
            return Ok(empresa); // Retorna un 200 OK con el objeto de la empresa
        }
    }

}