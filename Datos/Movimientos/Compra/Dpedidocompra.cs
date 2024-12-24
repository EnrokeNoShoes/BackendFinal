using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
using System.Buffers;

namespace ProyectoFinal.Datos{
    public class Dpedidocompra{

        ConexionBD cn = new ConexionBD();
        public async Task<Mpedidocompra> ObtenerPedidoCompraPorId(int id)
        {
            var pedido = new Mpedidocompra();

            using (var npgsql = new NpgsqlConnection(cn.cadenaSQL()))
            {
                await npgsql.OpenAsync();
                string consultaCabecera = @"
                    select pc.codpedidocompra,pc.codcomprobante, tp.numcomprobante as tipocomprobante,tp.descomprobante, pc.numcomprobante as numeroregistro, pc.fechapedido,pc.codestado,
                    em.numestado, em.desestado,  
                    pc.codsucursal, s.numsuc, s.dessucu, pc.codusu, u.nomusu
                    from pedidocompra pc
                    inner join sucursal s on pc.codsucursal = s.codsucursal
                    inner join comprobante tp on pc.codcomprobante = tp.codcomprobante
                    inner join estadomovimiento em on pc.codestado = em.codestado
                    inner join usuarios u on pc.codusu = u.codusu
                    where pc.codpedidocompra = @Id";

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
                            pedido.numcomprobante = (string)reader["numcomprobante"];
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
                string consultaDetalles = @"
                    select pcd.codpedidocompra, pcd.codproducto, prd.codigobarra , prd.desproducto, pcd.cantidad, pcd.costoulitmo
                    from pedidocompra_det pcd
                    inner join producto prd on pcd.codproducto = prd.codproducto where pcd.codpedidocompra = @Id";

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
                // Consulta para obtener las cabeceras de los pedidos
                string consultaCabecera = @"
                    select pc.codpedidocompra, pc.codcomprobante, tp.numcomprobante as tipocomprobante, tp.descomprobante, 
                        pc.numcomprobante as numeroregistro, pc.fechapedido, pc.codestado,
                        em.numestado, em.desestado,  
                        pc.codsucursal, s.numsuc, s.dessucu, pc.codusu, u.nomusu
                    from pedidocompra pc
                    inner join sucursal s on pc.codsucursal = s.codsucursal
                    inner join comprobante tp on pc.codcomprobante = tp.codcomprobante
                    inner join estadomovimiento em on pc.codestado = em.codestado
                    inner join usuarios u on pc.codusu = u.codusu";

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
                                Detalles = new List<Mdetallepedidocompra>() // Inicializar lista de detalles
                            };

                            pedidos.Add(pedido);
                        }
                    }
                }
                // Consulta para obtener los detalles, filtrando por codpedidocompra
                string consultaDetalles = @"
                    select pcd.codpedidocompra, pcd.codproducto, prd.codigobarra, prd.desproducto, 
                        pcd.cantidad, pcd.costoulitmo
                    from pedidocompra_det pcd
                    inner join producto prd on pcd.codproducto = prd.codproducto
                    where pcd.codpedidocompra = @codpedidocompra";

                using (var cmdDetalle = new NpgsqlCommand(consultaDetalles, npgsql))
                {
                    // Iterar sobre los pedidos y agregar sus detalles
                    foreach (var pedido in pedidos)
                    {
                        cmdDetalle.Parameters.Clear(); // Limpiar parámetros previos
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
                        string queryCabecera = @"
                            INSERT INTO pedidocompra (codpedidocompra, codcomprobante, numcomprobante, fechapedido, codestado, codsucursal, codusu)
                            VALUES (
                                (SELECT COALESCE(MAX(codpedidocompra), 0) + 1 FROM pedidocompra),
                                @codcomprobante,
                                @numcomprobante,
                                @fechapedido,
                                @codestado,
                                @codsucursal,
                                @codusu
                            )
                            RETURNING codpedidocompra";

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
                            string queryDetalle = @"
                                INSERT INTO pedidocompra_det (codpedidocompra, codproducto, cantidad, costoulitmo)
                                VALUES (@codpedidocompra, @codproducto, @cantidad, @costoulitmo)";

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

                        await transaction.CommitAsync(); // Confirma la transacción
                        return pedidoCompra.codpedidocompra; // Retorna el ID del pedido insertado
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(); // Revertir la transacción en caso de error
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
                        string query = @"
                            UPDATE pedidocompra
                            SET codestado = @codestado
                            WHERE codpedidocompra = @codpedidocompra";

                        using (var cmd = new NpgsqlCommand(query, npgsql))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@codpedidocompra", codpedidocompra);
                            cmd.Parameters.AddWithValue("@codestado", codestado);
                            cmd.Transaction = transaction;

                            int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                            await transaction.CommitAsync();
                            return filasAfectadas; // Retorna el número de filas actualizadas
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