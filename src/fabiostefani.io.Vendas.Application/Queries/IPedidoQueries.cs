using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using fabiostefani.io.Vendas.Application.Queries.ViewModels;

namespace fabiostefani.io.Vendas.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<CarrinhoViewModel> ObterCarrinhoCliente(Guid clienteId);
        Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clienteId);
    }
}