namespace ProyectoFinal.Fina{

    public class Mpresupuestocompra{
        public int codpresupuestocompra { get; set; }
        public int codproveedor { get; set; }
        public int codusu { get; set; }
        public int codsucursal { get; set; }
        public int codcomprobante { get; set; }
        public string numpresupuestoc { get; set; }
        public string fechapresupuesto { get; set; }
        public decimal totalexenta { get; set; }
        public decimal totalgravada { get; set; }
        public decimal totaliva { get; set; }
        public decimal totalpresupuesto { get; set; }
        public decimal totaldescuento { get; set; }
        public int codestado { get; set; }
        public List<Mpresupuestocompradet> Detalles { get; set; } = new List<Mpresupuestocompradet>();
    }

    public class Mpresupuestocompradet{
        public int codpresupuestocompra { get; set; }
        public int codproducto { get; set; }
        public decimal preciocompra { get; set; }
        public decimal precioneto { get; set; }
        public decimal cantidad { get; set; }
        public decimal costo_anterior { get; set; }
        public decimal costo_ultimo { get; set; }

    }
}