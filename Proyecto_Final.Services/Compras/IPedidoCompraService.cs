using Proyecto_Final.Shared.Compras;
using Proyecto_Final.Shared.Compras.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_Final.Services
{
    public interface IPedidoCompraService
    {
        Task<PedidoCompraDto> GetPedidoCompraByIdAsync(int id);
        Task<List<PedidoCompraDto>> GetAllPedidosCompraAsync();
        Task<int> InsertarPedidoCompra(PedidoCompra pedidoCompra);
        Task<int> ActualizarEstadoPedidoCompra(int codPedidoCompra, int codEstado);
    }
}
