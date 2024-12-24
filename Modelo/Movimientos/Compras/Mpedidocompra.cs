namespace ProyectoFinal.Modelo
{
    public class Mpedidocompra
    {
        public int codpedidocompra { get; set; }
        public int codsucursal { get; set; }
        public string numsuc { get; set; }
        public string dessuc { get; set; }
        public string fechapedido { get; set; }
        public int codcomprobante { get; set; }
        public string numcomprobante { get; set; }
        public string descomprobante { get; set; }
        public int codestado { get; set; }
        public string numestado { get; set; }
        public string desestado { get; set; }
        public int codusu { get; set; }
        public string nomusu {get; set;}
        public List<Mdetallepedidocompra> Detalles { get; set; } = new List<Mdetallepedidocompra>();
    }
    public class Mdetallepedidocompra
    {
        public int codpedidocompra { get; set; }
        public int codproducto { get; set; }
        public string codigobarra {get; set;}
        public string desproducto { get; set; }
        public decimal cantidad { get; set; }
        public decimal costoulitmo { get; set; }

    }
}
