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
            Mconsultapersona mconsultapersona = null; // Inicializar el objeto

            // Usar la conexión a PostgreSQL
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                // Ejecutar la función con el parámetro id para obtener un solo registro
                using (var cmd = new NpgsqlCommand("SELECT * FROM public.rucpersona_empresa(@id)", npgsql)) // Asegúrate de que tu función se llame correctamente
                {
                    cmd.Parameters.AddWithValue("@id", numdoc); // Agregar el parámetro

                    await npgsql.OpenAsync();

                    // Ejecutar la consulta
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) // Solo leer el primer registro
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
            return
            mconsultapersona; // Retornar el objeto encontrado o null si no se encontró
        }
    }


}