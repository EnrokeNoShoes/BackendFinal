using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
using System.Buffers;

namespace ProyectoFinal.Datos{
    public class Dempresa{

        ConexionBD cn = new ConexionBD();
       public async Task<string> InsertarEmpresa(Mempresa parametros) 
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT public.insertarempresa(@rucempresa, @razonsocial, @propietario, @ruc_ci, @nrotelefono, @direccion, @actividadeconomica)", npgsql))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@rucempresa", parametros.rucempresa);
                    cmd.Parameters.AddWithValue("@razonsocial", parametros.razonsocial);
                    cmd.Parameters.AddWithValue("@propietario", parametros.propietario);
                    cmd.Parameters.AddWithValue("@ruc_ci", parametros.ruc_ci);
                    cmd.Parameters.AddWithValue("@nrotelefono", parametros.nrotelefono);
                    cmd.Parameters.AddWithValue("@direccion", parametros.direccion);
                    cmd.Parameters.AddWithValue("@actividadeconomica", parametros.actividadeconomica);
                    await npgsql.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();                   
                    return result.ToString();
                }
            }
        }

        public async Task<string> EliminarEmpresa(Mempresa parametrosmpresa)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                // Cambiar la forma en que se llama a la función
                using (var cmd = new NpgsqlCommand("SELECT public.eliminar_empresa(@codempresa)", npgsql))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@codempresa", parametrosmpresa.codempresa);
                    
                    await npgsql.OpenAsync();
                    var resultado = await cmd.ExecuteScalarAsync();
                    return resultado?.ToString();
                }
            }
        }

        public async Task<List<Mempresa>> MostrarEmpresa() 
        {
            var lista = new List<Mempresa>();
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT * FROM public.mostarempresa()", npgsql))
                { 
                    await npgsql.OpenAsync();
                    
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var mempresa = new Mempresa();
                            mempresa.codempresa = (int)item["codempresa"];
                            mempresa.rucempresa = (string)item["rucempresa"];
                            mempresa.razonsocial = (string)item["razonsocial"];
                            mempresa.propietario = (string)item["propietario"];
                            mempresa.ruc_ci = (string)item["ruc_ci"];
                            mempresa.direccion = (string)item["direccion"];
                            mempresa.nrotelefono = (string)item["nrotelefono"];         
                            mempresa.actividadeconomica = (string)item["actividadeconomica"];
                            lista.Add(mempresa);
                        }
                    }
                }
            }

            return lista;
        }

        public async Task<Mempresa> MostrarEmpresaPorId(int id)
        {
            Mempresa mempresa = null;

            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT * FROM public.mostarempresa_por_id(@id)", npgsql)) 
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    await npgsql.OpenAsync();

                    // Ejecutar la consulta
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) 
                        {
                            mempresa = new Mempresa
                            {
                                rucempresa = reader["rucempresa"] as string,
                                razonsocial = reader["razonsocial"] as string,
                                propietario = reader["propietario"] as string,
                                ruc_ci = reader["ruc_ci"] as string,
                                direccion = reader["direccion"] as string,
                                nrotelefono = reader["nrotelefono"] as string,
                                actividadeconomica = reader["actividadeconomica"] as string,
                            };
                        }
                    }
                }
            }
            return mempresa;
        }

        public async Task ModificarEmpresa(Mempresa parametros, int codempresa)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT public.modificar_empresa(@codempresa, @rucempresa, @razonsocial, @propietario, @ruc_ci, @direccion, @nrotelefono, @actividadeconomica)", npgsql))
                {
                    cmd.Parameters.AddWithValue("@codempresa", codempresa);
                    cmd.Parameters.AddWithValue("@rucempresa", parametros.rucempresa);
                    cmd.Parameters.AddWithValue("@razonsocial", parametros.razonsocial);
                    cmd.Parameters.AddWithValue("@propietario", parametros.propietario);
                    cmd.Parameters.AddWithValue("@ruc_ci", parametros.ruc_ci);
                    cmd.Parameters.AddWithValue("@direccion", parametros.direccion);
                    cmd.Parameters.AddWithValue("@nrotelefono", parametros.nrotelefono);
                    cmd.Parameters.AddWithValue("@actividadeconomica", parametros.actividadeconomica);

                    await npgsql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> EmpresaExiste(int id)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT COUNT(1) FROM empresa WHERE codempresa = @codempresa", npgsql))
                {
                    cmd.Parameters.AddWithValue("@codempresa", id);
                    await npgsql.OpenAsync();

                    var result = await cmd.ExecuteScalarAsync();
                    return (long)result > 0;
                }
            }
        }

    }
}