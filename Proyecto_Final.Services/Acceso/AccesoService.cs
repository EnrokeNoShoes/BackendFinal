using Npgsql;
using Proyecto_Final.Data;
using Proyecto_Final.Data.Referenciales.Acceso;
using Proyecto_Final.Shared.Referenciales.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_Final.Shared.Referenciales;

namespace Proyecto_Final.Services.Acceso
{
    public class AccesoService : IAccesoService
    {
        private readonly string _connectionString;
        private readonly Usuario_sql _sqlQueries;

        public AccesoService(ProyectoFinal_Conexion conexion, Usuario_sql sqlQueries)
        {
            _connectionString = conexion.cadenaSQL();
            _sqlQueries = sqlQueries;
        }

        public async Task<Usuario> ValidarUsuario(string nombreUsuario, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                try
                {
                    var query = _sqlQueries.Select();
                    using (var cmd = new NpgsqlCommand(query, connection)) ;
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Usuario
                                {
                                    codusu = reader.GetInt32(0),
                                    nomusu = reader.GetString(1)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception($"Error al recuperar los datos. Consulta SQL");
                }

            }
            return null;
        }
    }
}
