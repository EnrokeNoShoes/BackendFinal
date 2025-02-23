namespace ProyectoFinal.Modelo{

    public class Mpresupuestocompra{
        public int codpresupuestocompra { get; set; }
        public int codproveedor { get; set; }
        public string numproveedor {get;set;}
        public string rucproveedor {get; set;}
        public string razonsocial {get;set;}
        public int codusu { get; set; }
        public string nomusu { get; set; }
        public int codsucursal { get; set;}
        public string numsucursal{get;set;}
        public string dessucursal{get;set;}
        public int codcomprobante { get; set; }
        public string numcomprobante {get; set;}
        public string descomprobante {get;set;}
        public string numpresupuestoc { get; set; }
        public string fechapresupuesto { get; set; }
        public decimal totalexenta { get; set; }
        public decimal totalgravada { get; set; }
        public decimal totaliva { get; set; }
        public decimal totalpresupuesto { get; set; }
        public decimal totaldescuento { get; set; }
        public int codmoneda {get;set;}
        public string nummoneda {get;set;}
        public string desmoneda {get;set;}
        public decimal cotizacion1 {get;set;}
        public decimal cotizacion2 {get;set;}
        public int codestado { get; set; }
        public string numestado {get;set;}
        public string desestado {get;set;}
        public List<Mpresupuestocompradet> Detalles { get; set; } = new List<Mpresupuestocompradet>();
    }

    public class Mpresupuestocompradet{
        public int codpresupuestocompra { get; set; }
        public int codproducto { get; set; }
        public string codigobarra {get;set;}
        public string desproducto {get;set;}
        public decimal preciocompra { get; set; }
        public decimal precioneto { get; set; }
        public decimal cantidad { get; set; }
        public decimal costo_anterior { get; set; }
        public decimal costo_ultimo { get; set; }

    }
}