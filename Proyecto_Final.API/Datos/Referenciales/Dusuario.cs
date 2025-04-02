using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
namespace ProyectoFinal.Datos{
    public class Dusuario
    {
        private readonly ConexionBD _conexionBD;

        public Dusuario() 
        {
            _conexionBD = new ConexionBD(); 
        }
        public async Task<Musuario> ValidarUsuario(string nombreUsuario, string password)
        {
            using (var npgsql = new NpgsqlConnection(_conexionBD.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT codusu, nomusu FROM usuarios WHERE nomusu = @nombreUsuario AND passusu = @password", npgsql))
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
                                nomusu = reader.GetString(1),                       
                            };
                        }
                    }
                }
            }
            return null;
        }

    }
}