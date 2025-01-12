using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;

namespace Proyecto_Final.Controller
{
    [ApiController]
    [Route("api/consultapersona")]
    public class Consultapersonacontroller : ControllerBase
    {
        Dconsultapersona funcion = new Dconsultapersona();
       
        [HttpGet("{numdoc}")]
        public async Task<ActionResult<Mconsultapersona>> Get(string numdoc)
        {
            var persona = await funcion.Mostrarpersona(numdoc);
            return persona == null ? NotFound(): Ok(persona);
        }
    }

}