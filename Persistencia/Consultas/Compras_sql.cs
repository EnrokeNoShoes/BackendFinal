using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ProyectoFinal.Persistencia
{
    public class PedidosCompras_sql : ISql
    {
        public string Insert()
        {
            return @"
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
        }

        public string InsertDet(){

            return @"INSERT INTO pedidocompra_det (codpedidocompra, codproducto, cantidad, costoulitmo)
                    VALUES (@codpedidocompra, @codproducto, @cantidad, @costoulitmo)";
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
                    INNER JOIN usuarios u ON pc.codusu = u.codusu order by 1";
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
        public string SelectDet(int option){

            if (option == 1){
                    return  @"
                    select pcd.codpedidocompra, pcd.codproducto, prd.codigobarra , prd.desproducto, pcd.cantidad, pcd.costoulitmo
                    from pedidocompra_det pcd
                    inner join producto prd on pcd.codproducto = prd.codproducto where pcd.codpedidocompra = @Id";
            }else if (option == 2){
                    return @"
                    select pcd.codpedidocompra, pcd.codproducto, prd.codigobarra, prd.desproducto, 
                    pcd.cantidad, pcd.costoulitmo
                    from pedidocompra_det pcd
                    inner join producto prd on pcd.codproducto = prd.codproducto
                    where pcd.codpedidocompra = @codpedidocompra";
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

        public string Delete(){
            return "";
        }
    }
    public class OrdenCompra_sql : ISql{

        public string Insert(){
            return "";
        }
        public string InsertDet(){
            return "";
        }
        public string Select(int option){
            return "";
        }
        public string SelectDet(int option){
            return "";
        }
        public string Update(){
            return "";
        }
        public string Delete(){
            return "";
        }

    }
    public class PresupuestoCompra_sql : ISql{

        public string Insert(){
            return @"INSERT INTO presupuestocompra (codpresupuestocompra, codproveedor, codusu, codsucursal, 
            codcomprobante, numpresupuestoc, fechapresupuesto, totalexenta,
            totalgravada, totaliva, totalpresupuesto, totaldescuento, codestado) 
            VALUES (SELECT COALESCE(MAX(codpresupuestocompra), 0) + 1 FROM codpresupuestocompra),
            @codproveedor, @codusu, @codsucursal, @codcomprobante, @numpresupuestoc, @fechapresupuesto,
            @totalexenta, @totalgravada, @totaliva, @totalpresupuesto, @totaldescuento, @codestado
            )
            RETURNING codpresupuestocompra";
        }
        public string InsertDet(){
            return @"INSERT INTO presupuestocompra_det (codpresupuestocompra, codproducto, 
            preciocompra, precioneto, cantidad, costo_anterior, costo_ultimo) VALUES (@codpresupuestocompra, @codproducto, @preciocompra, @precioneto, 
            @cantidad, @costo_anterior, @costo_ultimo);";
        }
        public string Select(int option){
            if (option == 1)
            {
                return @"
                    select * from presupuestocompra pc
                        inner join sucursal s on pc.codsucursal = s.codsucursal
                        inner join proveedor prv on pc.codproveedor = prv.codproveedor
                        inner join comprobante cp on pc.codcomprobante = cp.codcomprobante
                        inner join moneda mn on pc.codmoneda = mn.codmoneda
                        inner join estadomovimiento em on pc.codestado = em.codestado
                        INNER JOIN usuarios u ON pc.codusu = u.codusu
                    WHERE pc.codpresupuestocompra = @Id";
            }
            else if (option == 2)
            {
                return @"
                    select * from presupuestocompra pc
                        inner join sucursal s on pc.codsucursal = s.codsucursal
                        inner join proveedor prv on pc.codproveedor = prv.codproveedor
                        inner join moneda mn on pc.codmoneda = mn.codmoneda
                        inner join comprobante cp on pc.codcomprobante = cp.codcomprobante
                        inner join estadomovimiento em on pc.codestado = em.codestado
                        inner join usuarios u ON pc.codusu = u.codusu";
            }
            else if (option == 3)
            {
                return @"
                    SELECT codestado 
                    FROM presupuestocompra 
                    WHERE codpresupuestocompra = @codpresupuestocompra";
            }

            return string.Empty;;
        }
        public string SelectDet(int option){
             if (option == 1){
                    return  @"
                    select *
                    from presupuestocompra_det pcd
                    inner join producto prd on pcd.codproducto = prd.codproducto where pcd.codpresupuestocompra = @Id";
            }else if (option == 2){
                    return @"
                    select *
                    from presupuestocompra_det pcd
                    inner join producto prd on pcd.codproducto = prd.codproducto
                    where pcd.codpresupuestocompra = @codpresupuestocompra";
            }
            return string.Empty;
        }
        public string Update(){
             return @"
                UPDATE presupuestocompra
                SET codestado = @codestado
                WHERE codpresupuestocompra = @codpresupuestocompra";;
        }
        public string Delete(){
            return "";
        }

    }
    public class Compra_sql : ISql{

        public string Insert(){
            return "";
        }
        public string InsertDet(){
            return "";
        }
        public string Select(int option){
            return "";
        }
        public string SelectDet(int option){
            return "";
        }
        public string Update(){
            return "";
        }
        public string Delete(){
            return "";
        }

    }
}
