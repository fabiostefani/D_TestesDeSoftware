using System;
using System.Threading;
using System.Threading.Tasks;
using fabiostefani.io.Vendas.Application.Commands;
using fabiostefani.io.Vendas.Domain;
using MediatR;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace fabiostefani.io.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        // private readonly Guid _clienteId;
        // private readonly Guid _produtoId;
        // private readonly Pedido _pedido;
        private readonly AutoMocker _mocker;
        // private readonly PedidoCommandHandler _pedidoHandler;
        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            // _pedidoHandler = _mocker.CreateInstance<PedidoCommandHandler>();

            // _clienteId = Guid.NewGuid();
            // _produtoId = Guid.NewGuid();

            // _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
        }

        [Fact(DisplayName = "Adicionar Item Novo Pedido com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(),
                 Guid.NewGuid(), "Produto Teste", 2, 100);
            var mocker = new AutoMocker();
            var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();
            // _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            //var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(r=>r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
            // _mocker.GetMock<IPedidoRepository>().Verify(r=>r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            // _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            // //mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }
    }
}