using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
using System.Buffers;

namespace ProyectoFinal.Datos
{
    public class Dproducto
    {

        ConexionBD cn = new ConexionBD();
        Producto_SQL query = new Producto_SQL();
        public async Task<List<Mproducto>> GetProductoList()
        {
            var lista = new List<Mproducto>();
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                string consultaProducto = query.Select(1);
                using (var cmdProducto = new NpgsqlCommand(consultaProducto, npgsql))
                {
                    using (var readerProducto = await cmdProducto.ExecuteReaderAsync())
                    {
                        while (await readerProducto.ReadAsync())
                        {
                            var producto = new Mproducto
                            {
                                codproducto = Convert.ToInt32(readerProducto["codproducto"]),
                                codigobarra = (string)readerProducto["codigobarra"],
                                desproducto = (string)readerProducto["desproducto"],
                                numproveedor = (string)readerProducto["numproveedor"],
                                numdoc = (string)readerProducto["numdoc"],
                                razonsocial = (string)readerProducto["razonsocial"],
                                estado = Convert.ToInt32(readerProducto["estado"]),
                                numfamilia = (string)readerProducto["numfamilia"],
                                desfamilia = (string)readerProducto["desfamilia"],
                                nummarca = (string)readerProducto["nummarca"],
                                desmarca = (string)readerProducto["desmarca"],
                                numrubro = (string)readerProducto["numrubro"],
                                desrubro = (string)readerProducto["desrubro"],
                                numiva = (string)readerProducto["numiva"],
                                desiva = (string)readerProducto["desiva"],
                                costopromedio = (decimal)readerProducto["costopromedio"],
                                costoultimo = (decimal)readerProducto["costoultimo"]
                            };

                            lista.Add(producto);
                        }

                    }
                }
                await npgsql.CloseAsync();
                Console.WriteLine($"Estado al final (después de cerrar): {npgsql.State}"); 
            }
            return lista;
        }

        public async Task<List<Mproductosucursal>> GetProductoBuscador()
        {
            var lista = new List<Mproductosucursal>();
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                string consultaProducto = query.Select(2);
                using (var cmdProducto = new NpgsqlCommand(consultaProducto, npgsql))
                {
                    using (var readerProducto = await cmdProducto.ExecuteReaderAsync())
                    {
                        while (await readerProducto.ReadAsync())
                        {
                            var producto = new Mproductosucursal
                            {
                                codproducto = (int)readerProducto["codproducto"],
                                codigobarra = (string)readerProducto["codigobarra"],
                                desproducto = (string)readerProducto["desproducto"],
                                costoultimo = (decimal)readerProducto["costoultimo"],
                                codsucursal = (int)readerProducto["codsucursal"],
                                numsuc = (string)readerProducto["numsuc"],
                                dessucu = (string)readerProducto["dessucu"]
                            };

                            lista.Add(producto);
                        }

                    }
                }
                await npgsql.CloseAsync();
                Console.WriteLine($"Estado al final (después de cerrar): {npgsql.State}"); 
            }
            return lista;
        }
    }
}