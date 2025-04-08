using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Data.Compras
{
    public class PedidosCompras_sql
    {
        public string Insert()
        {
            return @"
            INSERT INTO pedidocompra (
                codpedidocompra, codcomprobante, numcomprobante, fechapedido, codestado, codsucursal, codusu
            ) VALUES (
                (SELECT COALESCE(MAX(codpedidocompra), 0) + 1 FROM pedidocompra),
                @codcomprobante,
                @numcomprobante,
                @fechapedido,
                @codestado,
                @codsucursal,
                @codusu
            )
            RETURNING codpedidocompra";
        }

        public string InsertDet()
        {
            return @"
            INSERT INTO pedidocompra_det (
                codpedidocompra, codproducto, cantidad, costoulitmo
            ) VALUES (
                @codpedidocompra, @codproducto, @cantidad, @costoulitmo
            )";
        }

        public string Select(int option)
        {
            if (option == 1)
            {
                return @"
                SELECT pc.codpedidocompra, pc.codcomprobante, tp.numcomprobante AS tipocomprobante, 
                       tp.descomprobante, pc.numcomprobante AS numeroregistro, pc.fechapedido, pc.codestado,
                       em.numestado, em.desestado,  
                       pc.codsucursal, s.numsuc, s.dessucu, pc.codusu, u.nomusu
                FROM pedidocompra pc
                INNER JOIN sucursal s ON pc.codsucursal = s.codsucursal
                INNER JOIN comprobante tp ON pc.codcomprobante = tp.codcomprobante
                INNER JOIN estadomovimiento em ON pc.codestado = em.codestado
                INNER JOIN usuarios u ON pc.codusu = u.codusu
                WHERE pc.codpedidocompra = @Id";
            }
            else if (option == 2)
            {
                return @"
                SELECT pc.codpedidocompra, pc.codcomprobante, tp.numcomprobante AS tipocomprobante, 
                       tp.descomprobante, pc.numcomprobante AS numeroregistro, pc.fechapedido, pc.codestado,
                       em.numestado, em.desestado,  
                       pc.codsucursal, s.numsuc, s.dessucu, pc.codusu, u.nomusu
                FROM pedidocompra pc
                INNER JOIN sucursal s ON pc.codsucursal = s.codsucursal
                INNER JOIN comprobante tp ON pc.codcomprobante = tp.codcomprobante
                INNER JOIN estadomovimiento em ON pc.codestado = em.codestado
                INNER JOIN usuarios u ON pc.codusu = u.codusu 
                ORDER BY pc.codpedidocompra";
            }
            else if (option == 3)
            {
                return @"
                SELECT codestado 
                FROM pedidocompra 
                WHERE codpedidocompra = @codpedidocompra";
            }

            return string.Empty;
        }

        public string SelectDet(int option)
        {
            if (option == 1)
            {
                return @"
                SELECT pcd.codpedidocompra, pcd.codproducto, prd.codigobarra, prd.desproducto, 
                       pcd.cantidad, pcd.costoulitmo
                FROM pedidocompra_det pcd
                INNER JOIN producto prd ON pcd.codproducto = prd.codproducto 
                WHERE pcd.codpedidocompra = @Id";
            }
            else if (option == 2)
            {
                return @"
                SELECT pcd.codpedidocompra, pcd.codproducto, prd.codigobarra, prd.desproducto, 
                       pcd.cantidad, pcd.costoulitmo
                FROM pedidocompra_det pcd
                INNER JOIN producto prd ON pcd.codproducto = prd.codproducto
                WHERE pcd.codpedidocompra = @codpedidocompra";
            }

            return string.Empty;
        }

        public string Update()
        {
            return @"
            UPDATE pedidocompra
            SET codestado = @codestado
            WHERE codpedidocompra = @codpedidocompra";
        }

        public string Delete()
        {
            return "";
        }
    }

}
