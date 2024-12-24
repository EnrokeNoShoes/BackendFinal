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
                    select pc.codpedidocompra,pc.codcomprobante, tp.numcomprobante,tp.descomprobante, pc.numcomprobante, pc.fechapedido,pc.codestado,
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
    }

}