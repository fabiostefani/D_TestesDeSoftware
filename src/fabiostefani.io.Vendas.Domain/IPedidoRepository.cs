using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using fabiostefani.io.Core.Data;
using fabiostefani.io.Vendas.Domain.Vouchers;

namespace fabiostefani.io.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);
        Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid clienteId);

        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
        void AdicionarItem(PedidoItem pedidoItem);
        void AtualizarItem(PedidoItem pedidoItem);
        void RemoverItem(PedidoItem pedidoItem);


        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}