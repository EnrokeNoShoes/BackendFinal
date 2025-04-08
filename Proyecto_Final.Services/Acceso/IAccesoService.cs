using Proyecto_Final.Shared.Referenciales;
using Proyecto_Final.Shared.Referenciales.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_Final.Services.Acceso
{
    public interface IAccesoService
    {
        Task<Usuario> ValidarUsuario(string nombreUsuario, string password);

    }
}
