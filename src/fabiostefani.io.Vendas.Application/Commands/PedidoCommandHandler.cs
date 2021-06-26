using System.Threading;
using System.Threading.Tasks;
using fabiostefani.io.Vendas.Application.Events;
using fabiostefani.io.Vendas.Domain;
using MediatR;

namespace fabiostefani.io.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediator _mediator;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }
        // public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        // {
        //     if (!ValidarComando(message)) return false;

        //     var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
        //     var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

        //     if (pedido == null)
        //     {
        //         pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
        //         pedido.AdicionarItem(pedidoItem);

        //         _pedidoRepository.Adicionar(pedido);
        //     }
        //     else
        //     {
        //         var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
        //         pedido.AdicionarItem(pedidoItem);

        //         if (pedidoItemExistente)
        //         {
        //             _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
        //         }
        //         else
        //         {
        //             _pedidoRepository.AdicionarItem(pedidoItem);
        //         }
                
        //         _pedidoRepository.Atualizar(pedido);
        //     }
            
        //     pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, 
        //         message.Nome, message.ValorUnitario, message.Quantidade));

        //     return await _pedidoRepository.UnitOfWork.Commit();
        // }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            // if (!ValidarComando(message)) return false;

            // var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            // var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

            // if (pedido == null)
            // {
            //     pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
            //     pedido.AdicionarItem(pedidoItem);

            //     _pedidoRepository.Adicionar(pedido);
            // }
            // else
            // {
            //     var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
            //     pedido.AdicionarItem(pedidoItem);

            //     if (pedidoItemExistente)
            //     {
            //         _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
            //     }
            //     else
            //     {
            //         _pedidoRepository.AdicionarItem(pedidoItem);
            //     }

            //     _pedidoRepository.Atualizar(pedido);
            // }

            

            // return await _pedidoRepository.UnitOfWork.Commit();
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
            pedido.AdicionarItem(pedidoItem);
            
            _pedidoRepository.Adicionar(pedido);

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Nome, message.ValorUnitario, message.Quantidade));
            return await _pedidoRepository.UnitOfWork.Commit();
        }

    }
}