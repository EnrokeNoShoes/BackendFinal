using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
using System.Buffers;
using ProyectoFinal.Fina;

namespace ProyectoFinal.Datos{
    public class Dpresupuestocompra{

        ConexionBD cn = new ConexionBD();
        PresupuestoCompra_sql query = new PresupuestoCompra_sql();
        public async Task<Mpresupuestocompra> ObtenerPresupuestoCompraPorId(int id)
        {
            var presupustocompra = new Mpresupuestocompra();

            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                string consultaCabecera = query.Select(1);

                using (var cmd = new NpgsqlCommand(consultaCabecera, npgsql))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            presupustocompra.codpresupuestocompra = (int)reader["codpresupuestocompra"];
                        }
                    }
                }

            }

        }
        


    }
}