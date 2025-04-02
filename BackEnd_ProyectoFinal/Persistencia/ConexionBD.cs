using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ProyectoFinal.Persistencia{
    public class ConexionBD
    {
        private string ConexionCadena = string.Empty;

        // Constructor para cargar la cadena de conexión desde el archivo de configuración
        public ConexionBD() 
        {
            // Construir la configuración para cargar el archivo appsettings.json
            var builder = new ConfigurationBuilder().SetBasePath
                (Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            // Obtener la cadena de conexión del archivo appsettings.json
            ConexionCadena = builder.GetSection("ConnectionStrings:conexionBD").Value;
        }

        // Método para devolver la cadena de conexión
        public string cadenaSQL() 
        {
            return ConexionCadena;
        }

        // Método para obtener una conexión a PostgreSQL
        public NpgsqlConnection GetPostgresConnection()
        {
            // Crear una nueva conexión usando Npgsql y la cadena de conexión
            return new NpgsqlConnection(ConexionCadena);
        }
    }


}