using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Custom;
using ProyectoFinal.Modelo;
using ProyectoFinal.Modelo.DTOs;
using Microsoft.AspNetCore.Authorization;
using ProyectoFinal.Datos;

namespace Proyecto_Final.Controllers
{
    [Route("api/acceso")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly Utilidades _utilidades;
        
        public AccesoController(Utilidades utilidades)
        {
            _utilidades = utilidades;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UsuariologinDTO login)
        {
            // Instancia de DUsuario para validar al usuario
            var usuarioService = new Dusuario();
            var usuario = await usuarioService.ValidarUsuario(login.NomUsu, login.PassUsu);

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
