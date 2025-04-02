using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Data
{

    public class ProyectoFinal_Conexion
    {
        private readonly string _conexionCadena;
        public ProyectoFinal_Conexion(IConfiguration configuration)
        {
            _conexionCadena = configuration.GetConnectionString("conexionBD");
        }
        public string cadenaSQL()
        {
            return _conexionCadena;
        }
        public NpgsqlConnection GetPostgresConnection()
        {
            Console.WriteLine("Intentando Conexion");
            
            return new NpgsqlConnection(_conexionCadena);
        }
    }
}
