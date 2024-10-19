using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Datos; // Asegúrate de importar el namespace correcto
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLogin login)
        {
            // Instanciar DUsuario directamente
            var usuarioService = new Dusuario();
            var usuario = await usuarioService.ValidarUsuario(login.NomUsu, login.PassUsu);
            
            if (usuario == null)
            {
                return Unauthorized(); // Credenciales inválidas
            }

            // Aquí puedes establecer una sesión si lo deseas
            return Ok(new { nomusu = usuario.nomusu }); // Devuelve el usuario o cualquier información que necesites
        }
    }

    public class UsuarioLogin
    {
        public string NomUsu { get; set; }
        public string PassUsu { get; set; }
    }
}

