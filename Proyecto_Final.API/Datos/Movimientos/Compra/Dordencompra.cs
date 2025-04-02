using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
using System.Buffers;

namespace ProyectoFinal.Datos{

     public class Dordencompra{

        ConexionBD cn = new ConexionBD();
        OrdenCompra_sql query = new OrdenCompra_sql();

        public async Task<Mordencompra> ObtenerOrdenCompraPorId(int id){
            var ordencompra = new Mordencompra();

            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL())){
                await npgsql.OpenAsync();
                string consultaCabecera = query.Select(1);
                using (var cmd = new NpgsqlCommand(consultaCabecera, npgsql))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            ordencompra.codorden = (int)reader["codorden"];
                        }
                    }
                }
                string consultaDetalles = query.SelectDet(1);
                using (var cmd = new NpgsqlCommand(consultaDetalles, npgsql))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var detalle = new Mordencompradet
                            {
                                codorden = (int)reader["codorden"],

                            };
                            ordencompra.Detalles.Add(detalle);
                        }
                    }
                }
            }
            return ordencompra;
        }

        public async Task<int> InsertarRegistro(Mordencompra ordencompra)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();

                using (var transaction = await npgsql.BeginTransactionAsync())
                {
                    try
                    {
                        string queryCabecera = query.Insert();
                        using (var cmdCabecera = new NpgsqlCommand(queryCabecera, npgsql))
                        {
                            cmdCabecera.CommandType = System.Data.CommandType.Text;
                            cmdCabecera.Parameters.AddWithValue("@codcomprobante", ordencompra.codcomprobante);
                            cmdCabecera.Parameters.AddWithValue("@numcomprobante", ordencompra.numorden);
                            cmdCabecera.Parameters.AddWithValue("@fechapedido", DateTime.Parse(ordencompra.fechaorden));
                            cmdCabecera.Parameters.AddWithValue("@codproveedor", ordencompra.codproveedor);
                            cmdCabecera.Parameters.AddWithValue("@codestado", ordencompra.totalexenta);
                            cmdCabecera.Parameters.AddWithValue("@codestado", ordencompra.totaliva);
                            cmdCabecera.Parameters.AddWithValue("@codestado", ordencompra.totalgravada);
                            cmdCabecera.Parameters.AddWithValue("@codestado", ordencompra.totaldescuento);
                            cmdCabecera.Parameters.AddWithValue("@codestado", ordencompra.totalorden);
                            cmdCabecera.Parameters.AddWithValue("@codestado", ordencompra.codestado);
                            cmdCabecera.Parameters.AddWithValue("@codsucursal", ordencompra.codsucursal);
                            cmdCabecera.Parameters.AddWithValue("@codusu", ordencompra.codusu);
                            cmdCabecera.Transaction = transaction;

                            ordencompra.codorden = (int)await cmdCabecera.ExecuteScalarAsync();
                        }

                        foreach (var detalle in ordencompra.Detalles)
                        {
                            string queryDetalle = query.InsertDet();
                            using (var cmdDetalle = new NpgsqlCommand(queryDetalle, npgsql))
                            {
                                cmdDetalle.CommandType = System.Data.CommandType.Text;
                                cmdDetalle.Parameters.AddWithValue("@codpresupuestocompra", ordencompra.codorden);
                                cmdDetalle.Parameters.AddWithValue("@codproducto", detalle.codproducto);
                                cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                                cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.preciocompra);
                                cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.precioneto);
                                cmdDetalle.Parameters.AddWithValue("@costoulitmo", detalle.costo_ultimo);
                                cmdDetalle.Transaction = transaction;
                                await cmdDetalle.ExecuteNonQueryAsync();
                            }
                        }

                        await transaction.CommitAsync();
                        return ordencompra.codorden;

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

         public async Task<int> ActualizarEstado(int codorden, int codestado)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                using (var transaction = await npgsql.BeginTransactionAsync())
                {
                    try
                    {
                        string consultaEstado = query.Select(3);

                        using (var cmdValidar = new NpgsqlCommand(consultaEstado, npgsql))
                        {
                            cmdValidar.Parameters.AddWithValue("@codorden", codorden);
                            cmdValidar.Transaction = transaction;

                            var estadoActual = await cmdValidar.ExecuteScalarAsync();

                            switch ((int)estadoActual)
                            {
                                case 2:
                                    throw new Exception("El presupuesto de compra ya se encuentra anulado.");
                                case 3:
                                    throw new Exception("El presupuesto de compra ya fue utilizado.");
                                case 4:
                                    throw new Exception("El presupuesto de compra no se puede utilizar ya supero los dias.");
                            }
                        }
                        string actulizarestado = query.Update();

                        using (var cmd = new NpgsqlCommand(actulizarestado, npgsql))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@codorden", codorden);
                            cmd.Parameters.AddWithValue("@codestado", codestado);
                            cmd.Transaction = transaction;

                            int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                            await transaction.CommitAsync();
                            return filasAfectadas;
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
    }
}