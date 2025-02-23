using ProyectoFinal.Modelo;
using ProyectoFinal.Persistencia;
using Npgsql; // Librería para PostgreSQL
using System.Data;
using System.Buffers;
using System.Resources;

namespace ProyectoFinal.Datos
{
    public class Dpresupuestocompra
    {

        ConexionBD cn = new ConexionBD();
        PresupuestoCompra_sql query = new PresupuestoCompra_sql();
        public async Task<Mpresupuestocompra> ObtenerPresupuestoCompraPorId(int id)
        {
            var presupuestocompra = new Mpresupuestocompra();

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
                            presupuestocompra.codpresupuestocompra = (int)reader["codpresupuestocompra"];
                            presupuestocompra.codcomprobante = (int)reader["codcomprobante"];
                            presupuestocompra.numcomprobante = (string)reader["numcomprobante"];
                            presupuestocompra.descomprobante = (string)reader["descomprobante"];
                            presupuestocompra.numpresupuestoc = (string)reader["numpresupuestoc"];
                            presupuestocompra.fechapresupuesto = (string)reader["fechapresupuesto"];
                            presupuestocompra.codsucursal = (int)reader["codsucursal"];
                            presupuestocompra.numsucursal = (string)reader["numsucursal"];
                            presupuestocompra.dessucursal = (string)reader["dessucursal"];
                            presupuestocompra.totalgravada = (decimal)reader["totalgravada"];
                            presupuestocompra.totalexenta = (decimal)reader["totalexenta"];
                            presupuestocompra.totaliva = (decimal)reader["totaliva"];
                            presupuestocompra.totaldescuento = (decimal)reader["totaldescuento"];
                            presupuestocompra.totalpresupuesto = (decimal)reader["totalpresupuesto"];
                            presupuestocompra.cotizacion1 = (decimal)reader["cotizacion1"];
                            presupuestocompra.cotizacion1 = (decimal)reader["cotizacion2"];
                            presupuestocompra.codmoneda = (int)reader["codmoneda"];
                            presupuestocompra.nummoneda = (string)reader["nummomeda"];
                            presupuestocompra.desmoneda = (string)reader["desmoneda"];
                            presupuestocompra.codmoneda = (int)reader["codmoneda"];
                            presupuestocompra.nummoneda = (string)reader["nummoneda"];
                            presupuestocompra.desmoneda = (string)reader["desmoneda"];
                            presupuestocompra.codusu = (int)reader["codusu"];
                            presupuestocompra.nomusu = (string)reader["nomusu"];
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
                            var detalle = new Mpresupuestocompradet
                            {
                                codpresupuestocompra = (int)reader["codpresupuestocompra"],
                                codproducto = (int)reader["codproducto"],
                                codigobarra = (string)reader["codigobarra"],
                                desproducto = (string)reader["desproducto"],
                                preciocompra = (decimal)reader["preciocompra"],
                                precioneto = (decimal)reader["precioneto"],
                                cantidad = (decimal)reader["cantidad"],
                                costo_anterior = (decimal)reader["costo_anterior"],
                                costo_ultimo = (decimal)reader["costo_ultimo"],
                            };
                            presupuestocompra.Detalles.Add(detalle);
                        }
                    }
                }
            }
            return presupuestocompra;
        }

        public async Task<int> InsertarRegistro(Mpresupuestocompra presupuestocompra)
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
                            cmdCabecera.Parameters.AddWithValue("@codcomprobante", presupuestocompra.codcomprobante);
                            cmdCabecera.Parameters.AddWithValue("@numcomprobante", presupuestocompra.numpresupuestoc);
                            cmdCabecera.Parameters.AddWithValue("@fechapedido", DateTime.Parse(presupuestocompra.fechapresupuesto));
                            cmdCabecera.Parameters.AddWithValue("@codproveedor", presupuestocompra.codproveedor);
                            cmdCabecera.Parameters.AddWithValue("@codestado", presupuestocompra.totalexenta);
                            cmdCabecera.Parameters.AddWithValue("@codestado", presupuestocompra.totaliva);
                            cmdCabecera.Parameters.AddWithValue("@codestado", presupuestocompra.totalgravada);
                            cmdCabecera.Parameters.AddWithValue("@codestado", presupuestocompra.totaldescuento);
                            cmdCabecera.Parameters.AddWithValue("@codestado", presupuestocompra.totalpresupuesto);
                            cmdCabecera.Parameters.AddWithValue("@codestado", presupuestocompra.codestado);
                            cmdCabecera.Parameters.AddWithValue("@codsucursal", presupuestocompra.codsucursal);
                            cmdCabecera.Parameters.AddWithValue("@codusu", presupuestocompra.codusu);
                            cmdCabecera.Transaction = transaction;

                            presupuestocompra.codpresupuestocompra = (int)await cmdCabecera.ExecuteScalarAsync();
                        }

                        foreach (var detalle in presupuestocompra.Detalles)
                        {
                            string queryDetalle = query.InsertDet();
                            using (var cmdDetalle = new NpgsqlCommand(queryDetalle, npgsql))
                            {
                                cmdDetalle.CommandType = System.Data.CommandType.Text;
                                cmdDetalle.Parameters.AddWithValue("@codpresupuestocompra", presupuestocompra.codpresupuestocompra);
                                cmdDetalle.Parameters.AddWithValue("@codproducto", detalle.codproducto);
                                cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                                cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.preciocompra);
                                cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.precioneto);
                                cmdDetalle.Parameters.AddWithValue("@costoulitmo", detalle.costo_anterior);
                                cmdDetalle.Parameters.AddWithValue("@costoulitmo", detalle.costo_ultimo);
                                cmdDetalle.Transaction = transaction;
                                await cmdDetalle.ExecuteNonQueryAsync();
                            }
                        }

                        await transaction.CommitAsync();
                        return presupuestocompra.codpresupuestocompra;

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

        public async Task<int> ActualizarEstado(int codpresupuestocompra, int codestado)
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
                            cmdValidar.Parameters.AddWithValue("@codpresupuestocompra", codpresupuestocompra);
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
                            cmd.Parameters.AddWithValue("@codpresupuestocompra", codpresupuestocompra);
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