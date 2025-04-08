using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Shared.Compras
{
    public class PedidoCompra
    {
        public int codpedidocompra { get; set; }
        public string numpedidocompra { get; set; }
        public int codsucursal { get; set; }
        public string fechapedido { get; set; }
        public int codcomprobante { get; set; }
        public int codestado { get; set; }
        public int codusu { get; set; }
        public List<PedidoCompraDetalle> detalles { get; set; } = new List<PedidoCompraDetalle>();
    }
    public class PedidoCompraDetalle
    {
        public int codpedidocompra { get; set; }
        public int codproducto { get; set; }
        public decimal cantidad { get; set; }
        public decimal costoulitmo { get; set; }

    }
}
