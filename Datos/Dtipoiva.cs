using System.Data;
using Npgsql;
using Proyecto_Final.Modelo;
using ProyectoFinal.Persistencia;

namespace ProyectoFinal.Datos{
    public class Dtipoiva{
        ConexionBD cn = new ConexionBD();

        public async Task<int> InsertarTipoiva(Mtipoiva parametros)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                string query =@"
                                INSERT INTO TIPOIVA (codiva, numiva, desiva, codusu,codempresa, coheficiente)
                                VALUES (
                                    (SELECT CASE WHEN MAX(codiva) IS NULL THEN 1 ELSE MAX(codiva)+1 END AS codiva FROM tipoiva),
                                    @numiva,
                                    @desiva,
                                    @codusu,
                                    @codempresa,
                                    @coheficiente
                                )";
                
                using (var cmd = new NpgsqlCommand(query, npgsql))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@numiva", parametros.numiva);
                    cmd.Parameters.AddWithValue("@desiva", parametros.desiva);
                    cmd.Parameters.AddWithValue("@codusu", parametros.codusu);
                    cmd.Parameters.AddWithValue("@codempresa", parametros.codempresa);
                    cmd.Parameters.AddWithValue("@coheficiente", parametros.coheficiente);
                    await npgsql.OpenAsync();
                    int filasafectadas = await cmd.ExecuteNonQueryAsync();
                    return filasafectadas;
                }
            }


        }

         public async Task<string> Eliminartipoiva(Mtipoiva parametros)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                // Cambiar la forma en que se llama a la función
                using (var cmd = new NpgsqlCommand("delete from tipoiva where codiva = @codiva", npgsql))
                {
                    cmd.CommandType = CommandType.Text; // Cambiar a Text porque no es un Stored Procedure
                    cmd.Parameters.AddWithValue("@codiva", parametros.codiva);
                    
                    await npgsql.OpenAsync();
                    var resultado = await cmd.ExecuteScalarAsync(); // Utiliza ExecuteScalar para obtener el mensaje
                    return resultado?.ToString();
                }
            }
        }

         public async Task<List<Mtipoiva>> MostrarTipoiva() 
        {
            var lista = new List<Mtipoiva>();

            // Usar la conexión a PostgreSQL
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                // Ejecutar la función sin parámetro
                using (var cmd = new NpgsqlCommand("SELECT numiva,desiva,coheficiente FROM tipoiva tv", npgsql))
                { 
                    await npgsql.OpenAsync();
                    
                    // Ejecutar la consulta
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var mtipoiva = new Mtipoiva();
                            mtipoiva.numiva = (string)item["numiva"];
                            mtipoiva.desiva = (string)item["desiva"];
                            mtipoiva.coheficiente = (decimal)item["coheficiente"];
                            lista.Add(mtipoiva);
                        }
                    }
                }
            }

            return lista;
        }

    }
}