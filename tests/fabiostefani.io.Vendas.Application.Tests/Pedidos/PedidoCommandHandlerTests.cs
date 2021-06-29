using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using fabiostefani.io.Core.DomainObjects;
using fabiostefani.io.Vendas.Application.Commands;
using fabiostefani.io.Vendas.Domain;
using MediatR;
using Moq;
using Moq.AutoMock;
using Xunit;
using fabiostefani.io.Vendas.Domain.Vouchers;

namespace fabiostefani.io.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        private readonly Guid _clienteId;
        private readonly Guid _produtoId;
        private readonly string _codigoVoucher;
        private readonly Pedido _pedido;
        private readonly AutoMocker _mocker;
        private readonly PedidoCommandHandler _pedidoHandler;
        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _pedidoHandler = _mocker.CreateInstance<PedidoCommandHandler>();

            _clienteId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();
            _codigoVoucher = "COD-15OFF";

            _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
        }

        #region Adicionar Item
        [Fact(DisplayName = "Adicionar Item Novo Pedido com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(),
                 Guid.NewGuid(), "Produto Teste", 2, 100);
            
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r=>r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);            
        }

        [Fact(DisplayName = "Adicionar Novo Item Pedido Rascunho com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_NovoItemAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange                        
            var pedidoItemExistente = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clienteId, Guid.NewGuid(), "Produto Teste", 2, 100);
            
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Existente ao Pedido Rascunho com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_ItemExistenteAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange                        
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clienteId, _produtoId, "Produto Xpto", 2, 100);

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);
            
            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
        #endregion

        #region Atualizar Item
        [Fact(DisplayName = "Atualizar Item Pedido Falha se não localizar o Pedido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AtualizarItem_PedidoExistente_DeveAdicionarEventoCasoNaoLocalizarPedido()
        {
            // Arrange
            var itemPedido = new PedidoItem(_produtoId, "Teste", 10, 10);
            _pedido.AdicionarItem(itemPedido);
            
            var pedidoCommand = new AtualizarItemPedidoCommand(_clienteId, _produtoId, 10);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Once);            
        }

        [Fact(DisplayName = "Atualizar Item Pedido Falha se não localizar o Item do Pedido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AtualizarItem_PedidoExistente_DeveAdicionarEventoCasoNaoLocalizarItemPedido()
        {
            // Arrange            
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            var pedidoCommand = new AtualizarItemPedidoCommand(_clienteId, _produtoId, 10);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Once);            
        }

        [Fact(DisplayName = "Atualizar Item Pedido com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AtualizarItem_Pedido_DeveExecutarComSucesso()
        {
            // Arrange
            var itemPedido = new PedidoItem(_produtoId, "Teste", 10, 10);
            _pedido.AdicionarItem(itemPedido);

            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterItemPorPedido(_pedido.Id, _produtoId)).Returns(Task.FromResult(itemPedido));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            var pedidoCommand = new AtualizarItemPedidoCommand(_clienteId, _produtoId, Pedido.MIN_UNIDADES_ITEM);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(Pedido.MIN_UNIDADES_ITEM, _pedido.PedidoItens.Sum(x => x.Quantidade));
            _mocker.GetMock<IPedidoRepository>().Verify(r=>r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r=>r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);            
        }

        [Fact(DisplayName = "Atualizar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AtualizarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(Guid.Empty, Guid.Empty, 0);
            
            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(3));
        }
        #endregion

        #region Remover Item
        [Fact(DisplayName = "Remover Item Pedido Falha se não localizar o Pedido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task RemoverItem_PedidoExistente_DeveAdicionarEventoCasoNaoLocalizarPedido()
        {
            // Arrange
            var itemPedido = new PedidoItem(_produtoId, "Teste", 10, 10);
            _pedido.AdicionarItem(itemPedido);
            
            var pedidoCommand = new RemoverItemPedidoCommand(_clienteId, _produtoId);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Once);            
        }

        [Fact(DisplayName = "Remover Item Pedido Falha se não localizar o Item do Pedido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task RemoverItem_PedidoExistente_DeveAdicionarEventoCasoNaoLocalizarItemPedido()
        {
            // Arrange            
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            var pedidoCommand = new RemoverItemPedidoCommand(_clienteId, _produtoId);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Once);            
        }

        [Fact(DisplayName = "Remover Item Pedido com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task RemoverItem_Pedido_DeveExecutarComSucesso()
        {
            // Arrange
            var itemPedido = new PedidoItem(_produtoId, "Teste", 10, 10);
            _pedido.AdicionarItem(itemPedido);

            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterItemPorPedido(_pedido.Id, _produtoId)).Returns(Task.FromResult(itemPedido));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            var pedidoCommand = new RemoverItemPedidoCommand(_clienteId, _produtoId);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r=>r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r=>r.RemoverItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);            
        }

        [Fact(DisplayName = "Remover Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task RemoverItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new RemoverItemPedidoCommand(Guid.Empty, Guid.Empty);
            
            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(2));
        }
        #endregion

        #region Aplicar Voucher
        [Fact(DisplayName = "Aplicar Voucher Item Pedido Falha se não localizar o Pedido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucherItem_PedidoExistente_DeveAdicionarEventoCasoNaoLocalizarPedido()
        {
            // Arrange
            var itemPedido = new PedidoItem(_produtoId, "Teste", 10, 10);
            _pedido.AdicionarItem(itemPedido);
            
            var pedidoCommand = new AplicarVoucherPedidoCommand(_clienteId, _codigoVoucher);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Once);            
        }

        [Fact(DisplayName = "Aplicar Voucher Pedido Falha se não localizar o Voucher")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_PedidoExistente_DeveAdicionarEventoCasoNaoLocalizarVoucher()
        {
            // Arrange            
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            var pedidoCommand = new AplicarVoucherPedidoCommand(_clienteId, _codigoVoucher);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Once);            
        }

        [Fact(DisplayName = "Aplicar Voucher Pedido Falha se Voucher Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_PedidoExistente_DeveFalharCasoVoucherInvalido()
        {
            // Arrange            
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            var pedidoCommand = new AplicarVoucherPedidoCommand(_clienteId, _codigoVoucher);

            var voucher = new Voucher(string.Empty, TipoDescontoVoucher.Valor, null, null, 0, DateTime.Now.AddDays(-1), false, true);

            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterVoucherPorCodigo(_codigoVoucher)).Returns(Task.FromResult(voucher));

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<DomainNotification>(), It.IsAny<CancellationToken>()), Times.Exactly(6));            
        }

        [Fact(DisplayName = "Aplicar Voucher Pedido com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_Pedido_DeveExecutarComSucesso()
        {
            // Arrange
            var itemPedido = new PedidoItem(_produtoId, "Teste", 10, 10);
            _pedido.AdicionarItem(itemPedido);

            var voucher = new Voucher(_codigoVoucher, TipoDescontoVoucher.Valor, 10, null, 10, DateTime.Now.AddDays(1), true, false);

            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterVoucherPorCodigo(_codigoVoucher)).Returns(Task.FromResult(voucher));
            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            var pedidoCommand = new AplicarVoucherPedidoCommand(_clienteId, _codigoVoucher);

            // Act            
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r=>r.Atualizar(It.IsAny<Pedido>()), Times.Once);            
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);            
        }

        [Fact(DisplayName = "Aplicar Voucher Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AplicarVoucherPedidoCommand(Guid.Empty, string.Empty);
            
            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(2));
        }
        #endregion
    }
}