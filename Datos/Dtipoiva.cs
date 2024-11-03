using System.Data;
using Npgsql;
using Proyecto_Final.Modelo;
using ProyectoFinal.Persistencia;

namespace ProyectoFinal.Datos{
    public class Dtipoiva{
        ConexionBD cn = new ConexionBD();

        /*public async Task<int> InsertarTipoiva(Mtipoiva parametros)
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
        }*/

        public async Task<int> InsertarTipoiva(Mtipoiva parametros)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                using (var transaction = await npgsql.BeginTransactionAsync())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO TIPOIVA (codiva, numiva, desiva, codusu, codempresa, coheficiente)
                            VALUES (
                                (SELECT CASE WHEN MAX(codiva) IS NULL THEN 1 ELSE MAX(codiva) + 1 END AS codiva FROM tipoiva),
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
                            cmd.Transaction = transaction; // Asocia el comando a la transacción

                            int filasafectadas = await cmd.ExecuteNonQueryAsync();
                            
                            await transaction.CommitAsync(); // Confirma la transacción si todo va bien
                            return filasafectadas;
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(); // Revierte la transacción en caso de error
                        Console.WriteLine($"Error: {ex.Message}");
                        throw; // Relanza la excepción para que pueda manejarse externamente si es necesario
                    }
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

        /* public async Task<List<Mtipoiva>> MostrarTipoiva() 
        {
            var lista = new List<Mtipoiva>();

            // Usar la conexión a PostgreSQL
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                // Ejecutar la función sin parámetro
                using (var cmd = new NpgsqlCommand("SELECT numiva,desiva,coheficiente,codempresa,codusu FROM tipoiva tv", npgsql))
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
                            mtipoiva.codempresa = (int)item["codempresa"];
                            mtipoiva.codempresa = (int)item["codusu"];
                            lista.Add(mtipoiva);
                        }
                    }
                }
            }

            return lista;
        }*/

        public async Task<List<Mtipoiva>> MostrarTipoiva(int codEmpresa) 
        {
            var lista = new List<Mtipoiva>();

            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT numiva, desiva, coheficiente FROM tipoiva WHERE codempresa = @codempresa", npgsql))
                {
                    cmd.Parameters.AddWithValue("@codempresa", codEmpresa);
                    await npgsql.OpenAsync();
                    
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



        public async Task<Mtipoiva> MostrarIvaporID(int id)
        {
            Mtipoiva mtipoiva = null; // Inicializar el objeto

            // Usar la conexión a PostgreSQL
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                // Ejecutar la función con el parámetro id para obtener un solo registro
                using (var cmd = new NpgsqlCommand("SELECT numiva,desiva,coheficiente FROM tipoiva where codiva = @id", npgsql)) // Asegúrate de que tu función se llame correctamente
                {
                    cmd.Parameters.AddWithValue("@id", id); // Agregar el parámetro

                    await npgsql.OpenAsync();

                    // Ejecutar la consulta
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) // Solo leer el primer registro
                        {
                            mtipoiva = new Mtipoiva
                            {
                                numiva = reader["numiva"] as string,
                                desiva = reader["desiva"] as string,
                                coheficiente = (decimal)reader["coheficiente"],
                            };
                        }
                    }
                }
            }
            return
             mtipoiva; // Retornar el objeto encontrado o null si no se encontró
        }


    }
}