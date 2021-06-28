using System.Linq;
using System;
using Xunit;
using fabiostefani.io.Core.DomainObjects;
using fabiostefani.io.Vendas.Domain.Vouchers;

namespace fabiostefani.io.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        private readonly Pedido _pedido;
        private readonly Guid _produtoId;
        public PedidoTests()
        {
            _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            _produtoId = Guid.NewGuid();
        }
        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            //ARRANGE
            
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

            //ACT
            _pedido.AdicionarItem(pedidoItem);

            //ASSERT
            Assert.Equal(200, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_itemExistente_DeveIncrementarUnidadesSomarValores()
        {
            //ARRANGE            
            
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 2, 100);
            _pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(_produtoId, "Produto Teste", 1, 100);
            //ACT

            _pedido.AdicionarItem(pedidoItem2);

            //ASSERT
            Assert.Equal(300, _pedido.ValorTotal);
            Assert.Equal(1, _pedido.PedidoItens.Count);
            Assert.Equal(3, _pedido.PedidoItens.FirstOrDefault(x => x.ProdutoId == _produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionar Item Pedido acima do permitido")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AdicionarItemPedido_UnidadesItensAcimaDoPermitido_DeveRetornarException()
        {
            //ARRANGE            
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            //ACT
            Assert.Throws<DomainException>(() => _pedido.AdicionarItem(pedidoItem));
            
            //ASSERT
            
        }    

        [Fact(DisplayName = "Adiciona Item Pedido Existente acima do permitido")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            //ARRANGE                        
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 1, 100);
            var pedidoItem2 = new PedidoItem(_produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 100);
            _pedido.AdicionarItem(pedidoItem);

            //ACT //ASSERT
            Assert.Throws<DomainException>(() => _pedido.AdicionarItem(pedidoItem2));
            
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            //ARRANGE                        
            var pedidoItemAtualizado = new PedidoItem(_produtoId, "Produto Teste", 1, 100);            

            //ACT //ASSERT
            Assert.Throws<DomainException>(() => _pedido.AtualizarItem(pedidoItemAtualizado));
            
        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            //ARRANGE                        
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 2, 100);
            _pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(_produtoId, "Produto Teste", 5, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            //ACT 
            _pedido.AtualizarItem(pedidoItemAtualizado);

            //ASSERT
            Assert.Equal(novaQuantidade, _pedido.PedidoItens.FirstOrDefault(z => z.ProdutoId == _produtoId).Quantidade);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            //ARRANGE                        
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(_produtoId, "Produto Teste", 3, 15);
            _pedido.AdicionarItem(pedidoItemExistente1);
            _pedido.AdicionarItem(pedidoItemExistente2);
            
            var pedidoItemAtualizado = new PedidoItem(_produtoId, "Produto Teste", 5, 15);
            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                              pedidoItemAtualizado.Quantidade * pedidoItemExistente2.ValorUnitario;

            //ACT 
            _pedido.AtualizarItem(pedidoItemAtualizado);

            //ASSERT
            Assert.Equal(totalPedido, _pedido.ValorTotal);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade acima do Permitido")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItensUnidadeAcimaDoPermitido_DeveRetornarException()
        {
            //ARRANGE                        
            var pedidoItemExistente1 = new PedidoItem(_produtoId, "Produto Teste", 3, 15);            
            _pedido.AdicionarItem(pedidoItemExistente1);            
            
            var pedidoItemAtualizado = new PedidoItem(_produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 15);
            
            //ACT  //ASSERT
            Assert.Throws<DomainException>(() => _pedido.AtualizarItem(pedidoItemAtualizado));            

        }

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void RemoverItemPedido_ItensNaoExisteNaLista_DeveRetornarException()
        {
            //ARRANGE                        
            var pedidoItemRemover = new PedidoItem(_produtoId, "Produto Teste", 3, 15);            
            
            //ACT  //ASSERT
            Assert.Throws<DomainException>(() => _pedido.RemoverItem(pedidoItemRemover));            

        }

        [Fact(DisplayName = "Remover Item Pedido Deve calcular valor total")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
        {
            //ARRANGE                        
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);            
            var pedidoItem2 = new PedidoItem(_produtoId, "Produto Teste", 3, 15);
            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);

            var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

            //ACT  
            _pedido.RemoverItem(pedidoItem1);

            //ASSERT
            Assert.Equal(totalPedido, _pedido.ValorTotal);

        }

        [Fact(DisplayName = "Aplicar voucher válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            // Arrange            
            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Valor, 15, null, 1,
                 DateTime.Now.AddDays(15), true, false);

            // Act
            var result = _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher Inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
        {
            // Arrange            
            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Valor, 15, null, 1, DateTime.Now.AddDays(-1), true, true);

            // Act
            var result = _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher tipo valor desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange            
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Valor, 15, null, 1, DateTime.Now.AddDays(10), true, false);

            var valorComDesconto = _pedido.ValorTotal - voucher.ValorDesconto;

            // Act
            _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorComDesconto, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange            
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-15-OFF", TipoDescontoVoucher.Porcentagem, null, 15, 1, DateTime.Now.AddDays(10), true, false);

            var valorDesconto = (_pedido.ValorTotal * voucher.PercentualDesconto) / 100;
            var valorTotalComDesconto = _pedido.ValorTotal - valorDesconto;

            // Act
            _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorTotalComDesconto, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher desconto excede valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
        {
            // Arrange            
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItem1);

            var voucher = new Voucher("PROMO-15-OFF", TipoDescontoVoucher.Valor, 300, null, 1, DateTime.Now.AddDays(10), true, false);

            // Act
            _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(0, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher recalcular desconto na modificação do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
        {
            // Arrange            
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItem1);

            var voucher = new Voucher("PROMO-15-OFF", TipoDescontoVoucher.Valor, 50, null, 1, DateTime.Now.AddDays(10), true, false);
            _pedido.AplicarVoucher(voucher);

            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 4, 25);

            // Act
            _pedido.AdicionarItem(pedidoItem2);

            // Assert
            var totalEsperado = _pedido.PedidoItens.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
            Assert.Equal(totalEsperado, _pedido.ValorTotal);
        }
    }
}