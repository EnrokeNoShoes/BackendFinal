namespace ProyectoFinal.Modelo{

    public class Mcompra{
        public int codcompra { get; set; }
        public int codsucursal { get; set; }
        public int codcomprobante { get; set; }
        public string numcompra { get; set; }
        public int codtipogasto { get; set; }
        public string fechacompra { get; set; }
        public int codusuautorizador { get; set; }
        public int codusu { get; set; }
        public decimal totalcompra { get; set; }
        public decimal totalgravada { get; set; }
        public decimal totalexenta { get; set; }
        public decimal totadescuento { get; set; }
        public decimal totaliva { get; set; }
        public List<Mcompradet> Detalles { get; set; } = new List<Mcompradet>();

    }

    public class Mcompradet{
        public int codcompra { get; set; }
        public int codproducto { get; set; }
        public int coddeposito { get; set; }
        public decimal cantidad { get; set; }
        public decimal preciocompra { get; set; }
        public decimal precioneto { get; set; }
        public decimal descuento { get; set; }
        public decimal costo_anterior { get; set; }
        public decimal costo_promedio { get; set; }
    }

}