using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Shared.Compras.DTO
{
    public class PedidoCompraDto
    {
        public int codpedidocompra { get; set; }
        public string numpedidocompra { get; set; }
        public string numsuc { get; set; }
        public string dessuc { get; set; }
        public string fechapedido { get; set; }
        public string numcomprobante { get; set; }
        public string descomprobante { get; set; }
        public string numestado { get; set; }
        public string desestado { get; set; }
        public string nomusu { get; set; }
        public List<PedidoCompraDetalleDto> detalles { get; set; } = new List<PedidoCompraDetalleDto>();
    }

    public class PedidoCompraDetalleDto
    {
        public string codigobarra { get; set; }
        public string desproducto { get; set; }
        public decimal cantidad { get; set; }
        public decimal costoulitmo { get; set; }
    }
}
