using Proyecto_Final.Shared.Compras;
using Proyecto_Final.Shared.Compras.DTO;
using Proyecto_Final.Data;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_Final.Services;
using System.Data;
using Proyecto_Final.Data.Compras;
using Proyecto_Final.Data.Utils;

namespace ProyectoFinal.Services
{
    public class PedidoCompraService : IPedidoCompraService
    {
        private readonly string _connectionString;
        private readonly PedidosCompras_sql _sqlQueries;

        public PedidoCompraService(ProyectoFinal_Conexion conexion, PedidosCompras_sql sqlQueries)
        {
            _connectionString = conexion.cadenaSQL();
            _sqlQueries = sqlQueries;
        }

        public async Task<PedidoCompraDto> GetPedidoCompraByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var pedidoCompra = await GetPedidoCompraById(connection, id);
                if (pedidoCompra == null)
                    return null;

                var detalles = await GetPedidoCompraDetallesById(connection, id);

                return new PedidoCompraDto
                {
                    codpedidocompra = pedidoCompra.codpedidocompra,
                    numpedidocompra = pedidoCompra.numpedidocompra,
                    numsuc = pedidoCompra.numsuc.ToString(),
                    dessuc = pedidoCompra.dessucu,
                    fechapedido = pedidoCompra.fechapedido.ToString(),
                    numcomprobante = pedidoCompra.numcomprobante.ToString(),
                    descomprobante = pedidoCompra.descomprobante,
                    numestado = pedidoCompra.numestado.ToString(),
                    desestado = pedidoCompra.desestado,
                    nomusu = pedidoCompra.nomusu,
                    detalles = detalles.Select(d => new PedidoCompraDetalleDto
                    {
                        codigobarra = d.codigobarra,
                        desproducto = d.desproducto,
                        cantidad = d.cantidad,
                        costoulitmo = d.costoulitmo
                    }).ToList()
                };
            }
        }

        public async Task<List<PedidoCompraDto>> GetAllPedidosCompraAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var pedidosCompra = await GetPedidosCompra(connection); 
                var pedidosCompraList = new List<PedidoCompraDto>();

                foreach (var pedido in pedidosCompra)
                {
                    var detalles = await GetPedidoCompraDetallesById(connection, pedido.codpedidocompra); 

                    
                    var detalleList = new List<PedidoCompraDetalleDto>();
                    foreach (var d in detalles)
                    {
                        detalleList.Add(new PedidoCompraDetalleDto
                        {
                            codigobarra = d.codigobarra,
                            desproducto = d.desproducto,
                            cantidad = Convert.ToDecimal(d.cantidad),
                            costoulitmo = Convert.ToDecimal(d.costoulitmo)
                        });
                    }

                    
                    pedidosCompraList.Add(new PedidoCompraDto
                    {
                        codpedidocompra = pedido.codpedidocompra,
                        numpedidocompra = pedido.numpedidocompra.ToString(),
                        numsuc = pedido.numsuc.ToString(),
                        dessuc = pedido.dessucu,
                        fechapedido = Convert.ToDateTime(pedido.fechapedido).ToString("yyyy-MM-dd"),
                        numcomprobante = pedido.numcomprobante.ToString(),
                        descomprobante = pedido.descomprobante,
                        numestado = pedido.numestado.ToString(),
                        desestado = pedido.desestado,
                        nomusu = pedido.nomusu,
                        detalles = detalleList
                    });

                }

                return pedidosCompraList;
            }
        }


        private async Task<List<dynamic>> GetPedidosCompra(NpgsqlConnection connection)
        {
            var query = _sqlQueries.Select(ConsultasEnum.ALLREGISTER);
            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var pedidos = new List<dynamic>();
                    while (await reader.ReadAsync())
                    {
                        pedidos.Add(new
                        {
                            codpedidocompra = reader["codpedidocompra"],
                            numpedidocompra = reader["numeroregistro"], 
                            numsuc = reader["numsuc"],
                            dessucu = reader["dessucu"],
                            fechapedido = reader["fechapedido"],
                            numcomprobante = reader["tipocomprobante"],
                            descomprobante = reader["descomprobante"],
                            numestado = reader["numestado"],
                            desestado = reader["desestado"],
                            nomusu = reader["nomusu"]
                        });
                    }
                    return pedidos;
                }
            }
        }

        private async Task<List<dynamic>> GetPedidoCompraDetallesById(NpgsqlConnection connection, int pedidoId)
        {
            var query = _sqlQueries.SelectDet(ConsultasEnum.ALLREGISTER);
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@codpedidocompra", pedidoId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var detalles = new List<dynamic>();
                    while (await reader.ReadAsync())
                    {
                        detalles.Add(new
                        {
                            codigobarra = reader["codigobarra"],
                            desproducto = reader["desproducto"],
                            cantidad = reader["cantidad"],
                            costoulitmo = reader["costoulitmo"]
                        });
                    }
                    return detalles;
                }
            }
        }

        private async Task<dynamic> GetPedidoCompraById(NpgsqlConnection connection, int id)
        {
            var query = _sqlQueries.Select(ConsultasEnum.PORID);
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new
                        {
                            codpedidocompra = reader["codpedidocompra"],
                            numpedidocompra = reader["numeroregistro"],
                            numsuc = reader["numsuc"],
                            dessucu = reader["dessucu"],
                            fechapedido = reader["fechapedido"],
                            numcomprobante = reader["tipocomprobante"],
                            descomprobante = reader["descomprobante"],
                            numestado = reader["numestado"],
                            desestado = reader["desestado"],
                            nomusu = reader["nomusu"]
                        };
                    }
                }
            }
            return null;
        }
        public async Task<int> InsertarPedidoCompra(PedidoCompra pedidoCompra)
        {
            using (var npgsql = new NpgsqlConnection(_connectionString))  
            {
                await npgsql.OpenAsync();
                using (var transaction = await npgsql.BeginTransactionAsync())
                {
                    try
                    {
                        // Insertar cabecera del pedido
                        string queryCabecera = _sqlQueries.Insert();

                        using (var cmdCabecera = new NpgsqlCommand(queryCabecera, npgsql))
                        {
                            cmdCabecera.CommandType = CommandType.Text;
                            cmdCabecera.Parameters.AddWithValue("@codcomprobante", pedidoCompra.codcomprobante);
                            cmdCabecera.Parameters.AddWithValue("@numcomprobante", pedidoCompra.numpedidocompra);
                            cmdCabecera.Parameters.AddWithValue("@fechapedido", DateTime.Parse(pedidoCompra.fechapedido));
                            cmdCabecera.Parameters.AddWithValue("@codestado", pedidoCompra.codestado);
                            cmdCabecera.Parameters.AddWithValue("@codsucursal", pedidoCompra.codsucursal);
                            cmdCabecera.Parameters.AddWithValue("@codusu", pedidoCompra.codusu);
                            cmdCabecera.Transaction = transaction;

                            pedidoCompra.codpedidocompra = (int)await cmdCabecera.ExecuteScalarAsync();
                        }

                        
                        foreach (var detalle in pedidoCompra.detalles)
                        {
                            string queryDetalle = _sqlQueries.InsertDet();

                            using (var cmdDetalle = new NpgsqlCommand(queryDetalle, npgsql))
                            {
                                cmdDetalle.CommandType = CommandType.Text;
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


        
        public async Task<int> ActualizarEstadoPedidoCompra(int codPedidoCompra, int codEstado)
        {
            using (var npgsql = new NpgsqlConnection(_connectionString))  
            {
                await npgsql.OpenAsync();
                using (var transaction = await npgsql.BeginTransactionAsync())
                {
                    try
                    {
                        
                        string consultaEstado = _sqlQueries.Select(ConsultasEnum.STATEREGISTER);

                        using (var cmdValidar = new NpgsqlCommand(consultaEstado, npgsql))
                        {
                            cmdValidar.Parameters.AddWithValue("@codpedidocompra", codPedidoCompra);
                            cmdValidar.Transaction = transaction;

                            var estadoActual = await cmdValidar.ExecuteScalarAsync();

                            
                            switch ((int)estadoActual)
                            {
                                case 2:
                                    throw new InvalidOperationException("El pedido de compra ya se encuentra anulado.");
                                case 3:
                                    throw new InvalidOperationException("El pedido de compra ya fue utilizado.");
                                case 4:
                                    throw new InvalidOperationException("El pedido de compra no se puede utilizar, ya superó los días permitidos.");
                            }
                        }

                        
                        string actualizaEstado = _sqlQueries.Update();

                        using (var cmd = new NpgsqlCommand(actualizaEstado, npgsql))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.AddWithValue("@codpedidocompra", codPedidoCompra);
                            cmd.Parameters.AddWithValue("@codestado", codEstado);
                            cmd.Transaction = transaction;

                            
                            int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                            
                            await transaction.CommitAsync();
                            return filasAfectadas;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        
                        await transaction.RollbackAsync();
                        throw new Exception("Hubo un error al actualizar el estado del pedido de compra. Consulte el registro de errores.");
                    }
                }
            }
        }
    }
}
