using System.Linq;
using System;
using Xunit;
using fabiostefani.io.Core.DomainObjects;

namespace fabiostefani.io.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Pedido Testes")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

            //ACT
            pedido.AdicionarItem(pedidoItem);

            //ASSERT
            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Pedido Testes")]
        public void AdicionarItemPedido_itemExistente_DeveIncrementarUnidadesSomarValores()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1, 100);
            //ACT

            pedido.AdicionarItem(pedidoItem2);

            //ASSERT
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItens.Count);
            Assert.Equal(3, pedido.PedidoItens.FirstOrDefault(x => x.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionar Item Pedido acima do permitido")]
        [Trait("CATEGORIA", "Pedido testes")]
        public void AdicionarItemPedido_UnidadesItensAcimaDoPermitido_DeveRetornarException()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());            
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            //ACT
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
            
            //ASSERT
            
        }    
    }
}