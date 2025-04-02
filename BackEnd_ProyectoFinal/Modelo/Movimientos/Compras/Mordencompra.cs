namespace ProyectoFinal.Modelo{

    public class Mordencompra{
        public int codorden { get; set; }
        public int codsucursal { get; set; }
        public int codproveedor { get; set; }
        public int codcomprobante { get; set; }
        public string numorden {get; set;}
        public string fechaorden { get; set; }
        public int codusu { get; set; }
        public int codmodalidad { get; set; }
        public int codestado { get; set; }
        public decimal totalorden { get; set; }
        public decimal totaliva { get; set; }
        public decimal totalexenta { get; set; }
        public decimal totaldescuento { get; set; }
        public decimal totalgravada { get; set; }
        public List<Mordencompradet> Detalles { get; set; } = new List<Mordencompradet>();
    }

    public class Mordencompradet{
        public int codorden { get; set; }
        public int codproducto { get; set; }
        public decimal preciocompra { get; set; }
        public decimal precioneto { get; set; }
        public decimal cantidad { get; set; }
        public decimal costo_ultimo { get; set; }
    }

}