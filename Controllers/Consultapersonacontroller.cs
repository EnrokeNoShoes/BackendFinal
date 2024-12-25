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
            var empresa = await funcion.Mostrarpersona(numdoc);
            if (empresa == null)
            {
                return NotFound();
            }
            return Ok(empresa);
        }
    }

}