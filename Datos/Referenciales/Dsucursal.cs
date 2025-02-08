using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
namespace ProyectoFinal.Datos{
    public class Dsucursal
    {
        ConexionBD cn = new ConexionBD();
        public async Task<List<Msucursal>> MostrarSucursal() 
        {
            var lista = new List<Msucursal>();
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new NpgsqlCommand("SELECT codsucursal, dessucu, numsuc FROM public.sucursal where tipo = 0", npgsql))
                { 
                    await npgsql.OpenAsync();
                    
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var msucursal = new Msucursal();
                            msucursal.codsucursal = (int)item["codsucursal"];
                            msucursal.numsuc = (string)item["numsuc"];
                            msucursal.dessucu = (string)item["dessucu"];
                            lista.Add(msucursal);
                        }
                    }
                }
            }

            return lista;
        }

    }
}