using System.Data;
using Npgsql;
using Proyecto_Final.Modelo;
using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;

namespace ProyectoFinal.Datos{
    public class Dconsultapersona{
        ConexionBD cn = new ConexionBD();
        public async Task<Mconsultapersona> Mostrarpersona(string numdoc)
        {
            Mconsultapersona mconsultapersona = null;

            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT * FROM public.rucpersona_empresa(@id)", npgsql)) 
                {
                    cmd.Parameters.AddWithValue("@id", numdoc);

                    await npgsql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            mconsultapersona = new Mconsultapersona
                            {
                                ruc = reader["ruc"] as string,
                                apellido = reader["apellido"] as string,
                                nombre = reader["nombre"] as string,
                                digitoverificador = reader["identificador"] as string,
                                estado = reader["estado"] as string,
                            };
                        }
                    }
                }
            }
            return mconsultapersona;
        }
    }


}