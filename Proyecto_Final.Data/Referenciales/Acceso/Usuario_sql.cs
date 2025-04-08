using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Data.Referenciales.Acceso
{
    public class Usuario_sql
    {
        public string Select()
        {
            return @"SELECT codusu, nomusu FROM usuarios WHERE nomusu = @nombreUsuario AND passusu = @password";
        }
    }
}
