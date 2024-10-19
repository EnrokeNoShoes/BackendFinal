using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
namespace ProyectoFinal.Datos{
    public class Dusuario
    {
         private readonly ConexionBD _conexionBD; // Asegúrate de que esto esté configurado correctamente.

        public Dusuario() // Constructor sin parámetros, o si necesita una conexión, inyecta la conexión
        {
            _conexionBD = new ConexionBD(); // Puedes cambiar esto para recibir una instancia a través de inyección si es necesario
        }
        public async Task<Musuario> ValidarUsuario(string nombreUsuario, string password)
        {
            using (var npgsql = new NpgsqlConnection(_conexionBD.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT codusu, codempresa, nomusu FROM usuarios WHERE nomusu = @nombreUsuario AND passusu = @password", npgsql))
                {
                    cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Musuario
                            {
                                codusu = reader.GetInt32(0),                              
                                codempresa = reader.GetInt32(1),
                                nomusu = reader.GetString(2),
                                // Otras propiedades...
                            };
                        }
                    }
                }
            }
            return null; // Usuario no encontrado o credenciales inválidas
        }
    }
}