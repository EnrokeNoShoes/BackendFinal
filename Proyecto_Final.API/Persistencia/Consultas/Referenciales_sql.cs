namespace ProyectoFinal.Persistencia
{
    public class Sucursal : ISql
    {
        public string Delete()
        {
            throw new NotImplementedException();
        }

        public string Insert()
        {
            return @"insert into sucursal(codsucursal, numsuc, dessucu, telfsuc, codsucpertenece, tipo)
                values(@codsucursal, @numsuc, @dessuc, @telsuc, @codsucpertenece, @tipo)";
        }

        public string InsertDet()
        {
            throw new NotImplementedException();
        }

        public string Select(int option)
        {
            throw new NotImplementedException();
        }

        public string SelectDet(int option)
        {
            throw new NotImplementedException();
        }

        public string Update()
        {
            throw new NotImplementedException();
        }
    }

    public class Producto_SQL : ISql{
        public string Delete()
        {
            throw new NotImplementedException();
        }

        public string Insert()
        {
            return @"insert into sucursal(codsucursal, numsuc, dessucu, telfsuc, codsucpertenece, tipo)
                values(@codsucursal, @numsuc, @dessuc, @telsuc, @codsucpertenece, @tipo)";
        }

        public string InsertDet()
        {
            throw new NotImplementedException();
        }

        public string Select(int option)
        {
            string  query = "";
            if (option == 1){
                //Para recupera la lista de productos
                query = @"select p.codproducto, p.codigobarra, p.desproducto , 
                        p.estado, p.codiva , ti.numiva , ti.desiva , ti.coheficiente ,
                        prv.codproveedor , prv.numproveedor , prv.numdoc , prv.apellidos || ' ' || prv.nombres as razonsocial,
                        p.codmarca , m.nummarca , m.desmarca ,
                        p.codfamilia , f.numfamilia , f.desfamilia ,
                        p.codrubro , r.numrubro , r.desrubro ,
                        p.codunidadmedida, um.desunidadmedida , um.numunidadmedida,
                        p.costoultimo , p.costopromedio 
                        from producto p
                        inner join proveedor prv on p.codproveedor = prv.codproveedor
                        inner join tipoiva ti on p.codiva = ti.codiva
                        inner join unidadmedida um on p.codunidadmedida = um.codunidadmedida
                        inner join rubro r on p.codrubro = r.codrubro 
                        inner join familia f on p.codfamilia = f.codfamilia
                        inner join marca m on p.codmarca  = m.codmarca";

            }else if(option == 2){
                //Para el buscador de productos
                query = @"select ps.codproducto, p.codigobarra, p.desproducto, p.costoultimo,
                        ps.codsucursal, s.numsuc , s.dessucu
                        from productosucursal ps 
                        inner join producto p on ps.codproducto =  p.codproducto
                        inner join sucursal s on ps.codsucursal = s.codsucursal ";
            }else if(option == 3) {
                //Para recuperar en el modificar
                query = @"select p.codproducto, p.codigobarra, p.desproducto , 
                        p.estado, p.codiva , ti.numiva , ti.desiva , ti.coheficiente ,
                        prv.codproveedor , prv.numproveedor , prv.numdoc , prv.apellidos || ' ' || prv.nombres as razonsocial,
                        p.codmarca , m.nummarca , m.desmarca ,
                        p.codfamilia , f.numfamilia , f.desfamilia ,
                        p.codrubro , r.numrubro , r.desrubro ,
                        p.codunidadmedida, um.desunidadmedida , um.numunidadmedida,
                        p.costoultimo , p.costopromedio 
                        from producto p
                        inner join proveedor prv on p.codproveedor = prv.codproveedor
                        inner join tipoiva ti on p.codiva = ti.codiva
                        inner join unidadmedida um on p.codunidadmedida = um.codunidadmedida
                        inner join rubro r on p.codrubro = r.codrubro 
                        inner join familia f on p.codfamilia = f.codfamilia
                        inner join marca m on p.codmarca  = m.codmarca
                        where p.codproducto = @Id";
            }
            return query;
        }

        public string SelectDet(int option)
        {
            throw new NotImplementedException();
        }

        public string Update()
        {
            throw new NotImplementedException();
        }

    }
}