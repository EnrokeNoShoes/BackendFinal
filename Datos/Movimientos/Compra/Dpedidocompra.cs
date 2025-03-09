using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
using System.Buffers;

namespace ProyectoFinal.Datos{
    public class Dpedidocompra{

        ConexionBD cn = new ConexionBD();
        PedidosCompras_sql query = new PedidosCompras_sql();
        public async Task<Mpedidocompra> ObtenerPedidoCompraPorId(int id)
        {
            var pedido = new Mpedidocompra();

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
                            pedido.codpedidocompra = (int)reader["codpedidocompra"];
                            pedido.codsucursal = (int)reader["codsucursal"];
                            pedido.numsuc = (string)reader["numsuc"];
                            pedido.dessuc = (string)reader["dessucu"];
                            pedido.fechapedido = reader["fechapedido"].ToString();
                            pedido.codcomprobante = (int)reader["codcomprobante"];
                            pedido.numcomprobante = (string)reader["tipocomprobante"];
                            pedido.numcomprobantepc = (string)reader["numeroregistro"];
                            pedido.descomprobante = (string)reader["descomprobante"];
                            pedido.codestado = (int)reader["codestado"];
                            pedido.numestado = (string)reader["numestado"];
                            pedido.desestado = (string)reader["desestado"];
                            pedido.codusu = (int)reader["codusu"];
                            pedido.nomusu = (string)reader["nomusu"];
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
                            var detalle = new Mdetallepedidocompra
                            {
                                codpedidocompra = (int)reader["codpedidocompra"],	
                                codproducto = (int)reader["codproducto"],
                                codigobarra = (string)reader["codigobarra"],
                                desproducto = (string)reader["desproducto"],
                                cantidad = (decimal)reader["cantidad"],
                                costoulitmo = (decimal)reader["costoulitmo"]
                            };
                            pedido.Detalles.Add(detalle);
                        }
                    }
                }
            }
            return pedido;
        }    
        public async Task<List<Mpedidocompra>> ObtenerPedidoCompraLista()
        {
            var pedidos = new List<Mpedidocompra>();
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                string consultaCabecera = query.Select(2);

                using (var cmdCabecera = new NpgsqlCommand(consultaCabecera, npgsql))
                {
                    using (var readerCabecera = await cmdCabecera.ExecuteReaderAsync())
                    {
                        while (await readerCabecera.ReadAsync())
                        {
                            var pedido = new Mpedidocompra
                            {
                                codpedidocompra = (int)readerCabecera["codpedidocompra"],
                                codsucursal = (int)readerCabecera["codsucursal"],
                                numsuc = (string)readerCabecera["numsuc"],
                                dessuc = (string)readerCabecera["dessucu"],
                                fechapedido = readerCabecera["fechapedido"].ToString(),
                                codcomprobante = (int)readerCabecera["codcomprobante"],
                                numcomprobante = (string)readerCabecera["tipocomprobante"],
                                numcomprobantepc = (string)readerCabecera["numeroregistro"],
                                descomprobante = (string)readerCabecera["descomprobante"],
                                codestado = (int)readerCabecera["codestado"],
                                numestado = (string)readerCabecera["numestado"],
                                desestado = (string)readerCabecera["desestado"],
                                codusu = (int)readerCabecera["codusu"],
                                nomusu = (string)readerCabecera["nomusu"],
                                Detalles = new List<Mdetallepedidocompra>()
                            };

                            pedidos.Add(pedido);
                        }
                    }
                }
                string consultaDetalles = query.SelectDet(2);
                using (var cmdDetalle = new NpgsqlCommand(consultaDetalles, npgsql))
                {
                    foreach (var pedido in pedidos)
                    {
                        cmdDetalle.Parameters.Clear();
                        cmdDetalle.Parameters.AddWithValue("@codpedidocompra", pedido.codpedidocompra);

                        using (var readerDetalle = await cmdDetalle.ExecuteReaderAsync())
                        {
                            while (await readerDetalle.ReadAsync())
                            {
                                var detalle = new Mdetallepedidocompra
                                {
                                    codpedidocompra = (int)readerDetalle["codpedidocompra"],
                                    codproducto = (int)readerDetalle["codproducto"],
                                    codigobarra = (string)readerDetalle["codigobarra"],
                                    desproducto = (string)readerDetalle["desproducto"],
                                    cantidad = (decimal)readerDetalle["cantidad"],
                                    costoulitmo = (decimal)readerDetalle["costoulitmo"]
                                };
                                pedido.Detalles.Add(detalle);
                            }
                        }
                    }
                }
            }
            return pedidos;
        }
        public async Task<int> InsertarPedidoCompra(Mpedidocompra pedidoCompra)
        {
            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                using (var transaction = await npgsql.BeginTransactionAsync())
                {
                    try
                    {
                        // Insertar cabecera del pedido
                        string queryCabecera = query.Insert();

                        using (var cmdCabecera = new NpgsqlCommand(queryCabecera, npgsql))
                        {
                            cmdCabecera.CommandType = System.Data.CommandType.Text;
                            cmdCabecera.Parameters.AddWithValue("@codcomprobante", pedidoCompra.codcomprobante);
                            cmdCabecera.Parameters.AddWithValue("@numcomprobante", pedidoCompra.numcomprobantepc);
                            cmdCabecera.Parameters.AddWithValue("@fechapedido", DateTime.Parse(pedidoCompra.fechapedido));
                            cmdCabecera.Parameters.AddWithValue("@codestado", pedidoCompra.codestado);
                            cmdCabecera.Parameters.AddWithValue("@codsucursal", pedidoCompra.codsucursal);
                            cmdCabecera.Parameters.AddWithValue("@codusu", pedidoCompra.codusu);
                            cmdCabecera.Transaction = transaction;

                            pedidoCompra.codpedidocompra = (int)await cmdCabecera.ExecuteScalarAsync();
                        }

                        // Insertar detalles del pedido
                        foreach (var detalle in pedidoCompra.Detalles)
                        {
                            string queryDetalle = query.InsertDet();

                            using (var cmdDetalle = new NpgsqlCommand(queryDetalle, npgsql))
                            {
                                cmdDetalle.CommandType = System.Data.CommandType.Text;
                                cmdDetalle.Parameters.AddWithValue("@codpedidocompra", pedidoCompra.codpedidocompra);
                                cmdDetalle.Parameters.AddWithValue("@codproducto", detalle.codproducto);
                                cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                                cmdDetalle.Parameters.AddWithValue("@costoulitmo", detalle.costoulitmo);
                                cmdDetalle.Transaction = transaction;
                                await cmdDetalle.ExecuteNonQueryAsync();
                            }
                        }

                        await transaction.CommitAsync();
                        return pedidoCompra.codpedidocompra;
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
        public async Task<int> ActualizarEstadoPedidoCompra(int codpedidocompra, int codestado)
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
                            cmdValidar.Parameters.AddWithValue("@codpedidocompra", codpedidocompra);
                            cmdValidar.Transaction = transaction;

                            var estadoActual = await cmdValidar.ExecuteScalarAsync();

                            switch ((int)estadoActual){
                                case 2:
                                    throw new Exception("El pedido de compra ya se encuentra anulado.");
                                case 3: 
                                    throw new Exception("El pedido de compra ya fue utilizado.");
                                case 4:
                                    throw new Exception("El pedido de compra no se puede utilizar ya supero los dias.");
                            }
                        }
                        string actulizarestado = query.Update();

                        using (var cmd = new NpgsqlCommand(actulizarestado, npgsql))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@codpedidocompra", codpedidocompra);
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
                        throw;
                    }
                }
            }
        }


        
    }

}