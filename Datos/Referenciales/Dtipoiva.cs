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
                await npgsql.OpenAsync();
                using (var transaction = await npgsql.BeginTransactionAsync())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO TIPOIVA (codiva, numiva, desiva, codusu, coheficiente)
                            VALUES (
                                (SELECT CASE WHEN MAX(codiva) IS NULL THEN 1 ELSE MAX(codiva) + 1 END AS codiva FROM tipoiva),
                                @numiva,
                                @desiva,
                                @codusu,
                                @coheficiente
                            )";

                        using (var cmd = new NpgsqlCommand(query, npgsql))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@numiva", parametros.numiva);
                            cmd.Parameters.AddWithValue("@desiva", parametros.desiva);
                            cmd.Parameters.AddWithValue("@codusu", parametros.codusu);
                            cmd.Parameters.AddWithValue("@coheficiente", parametros.coheficiente);
                            cmd.Transaction = transaction; 

                            int filasafectadas = await cmd.ExecuteNonQueryAsync();
                            
                            await transaction.CommitAsync(); 
                            return filasafectadas;
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(); 
                        Console.WriteLine($"Error: {ex.Message}");
                        throw; 
                    }
                }
            }
        }

        public async Task<string> Eliminartipoiva(int codiva)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                using (var transaction = await npgsql.BeginTransactionAsync())
                {
                    try
                    {
                        using (var cmd = new NpgsqlCommand("DELETE FROM tipoiva WHERE codiva = @codiva", npgsql))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@codiva", codiva);
                            cmd.Transaction = transaction;

                            int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                            
                            if (filasAfectadas > 0)
                            {
                                await transaction.CommitAsync();
                                return "Eliminado correctamente";
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                return "No se encontró el registro";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<List<Mtipoiva>> MostrarTipoiva() 
        {
            var lista = new List<Mtipoiva>();

            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT numiva,desiva,coheficiente,codusu FROM tipoiva tv", npgsql))
                { 
                    await npgsql.OpenAsync();

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var mtipoiva = new Mtipoiva();
                            mtipoiva.numiva = (string)item["numiva"];
                            mtipoiva.desiva = (string)item["desiva"];
                            mtipoiva.coheficiente = (decimal)item["coheficiente"];
                            mtipoiva.codusu = (int)item["codusu"];
                            lista.Add(mtipoiva);
                        }
                    }
                }
            }

            return lista;
        }
        public async Task<Mtipoiva> MostrarIvaporID(int id)
        {
            Mtipoiva mtipoiva = null;

            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT numiva,desiva,coheficiente FROM tipoiva where codiva = @id", npgsql))
                {
                    cmd.Parameters.AddWithValue("@id", id); 

                    await npgsql.OpenAsync();

                    // Ejecutar la consulta
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) 
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
             mtipoiva; 
        }


    }
}