using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos;
using ProyectoFinal.Modelo;


namespace ProyectoFinal.Controllers{


    [ApiController]
    [Route("api/empresa")]
    public class EmpresaController : ControllerBase
    {
        [HttpPost]
        public async Task Post([FromBody] Mempresa parametros)
        {
            var funcion = new Dempresa();
            await funcion.InsertarEmpresa(parametros);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var funcion = new Dempresa();
            var paramateros = new Mempresa();
            paramateros.codempresa = id;
            await funcion.EliminarEmpresa(paramateros);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mempresa>> Get(int id)
        {
            var funcion = new Dempresa();
            var empresa = await funcion.MostrarEmpresaPorId(id);
            if (empresa == null) 
            {
                return NotFound();
            }
            return Ok(empresa);
        }

        [HttpGet]
        public async Task<ActionResult<List<Mempresa>>> Get()
        {
            var funcion = new Dempresa();
            var lista = await funcion.MostrarEmpresa();
            return lista;
        }   

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Mempresa parametros)
        {
            
            var funcion = new Dempresa();
            if (!await funcion.EmpresaExiste(id))
            {
                return NotFound($"No se encontró una empresa con ID {id}.");
            }
            try
            {
                await funcion.ModificarEmpresa(parametros, id);
            }
            catch (Exception ex)
            {
                return NotFound($"No se pudo modificar la empresa: {ex.Message}");
            }
            return NoContent();
        } 
    }
}