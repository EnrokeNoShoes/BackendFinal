using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Custom;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Services.Acceso;
using Proyecto_Final.Shared.Referenciales;

namespace Proyecto_Final.Controllers
{
    [Route("api/acceso")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly Utilidades _utilidades;
        private readonly IAccesoService _accesoService;

        public AccesoController(Utilidades utilidades, IAccesoService accesoService)
        {
            _utilidades = utilidades;
            _accesoService = accesoService; 
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Usuario login)
        {
            // Instancia de DUsuario para validar al usuario
            var usuarioService = new Usuario();
            var usuario = await _accesoService.ValidarUsuario(login.nomusu, login.passusu);

            if (usuario == null)
            {
                return Unauthorized(); // Credenciales inválidas
            }

            // Genera el JWT usando la clase Utilidades
            var token = _utilidades.generarJWT(usuario);

            return Ok(new { isSuccess = true, token }); // Devuelve el token
        }
    }
}
