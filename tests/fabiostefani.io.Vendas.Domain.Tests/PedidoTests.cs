using System.Linq;
using System;
using Xunit;
using fabiostefani.io.Core.DomainObjects;

namespace fabiostefani.io.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
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
        [Trait("Categoria", "Vendas - Pedido")]
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
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AdicionarItemPedido_UnidadesItensAcimaDoPermitido_DeveRetornarException()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());            
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            //ACT
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
            
            //ASSERT
            
        }    

        [Fact(DisplayName = "Adiciona Item Pedido Existente acima do permitido")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());  
            var produtoId = Guid.NewGuid();          
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 1, 100);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 100);
            pedido.AdicionarItem(pedidoItem);

            //ACT //ASSERT
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
            
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());  
            var produtoId = Guid.NewGuid();          
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 1, 100);            

            //ACT //ASSERT
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
            
        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());  
            var produtoId = Guid.NewGuid();          
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            //ACT 
            pedido.AtualizarItem(pedidoItemAtualizado);

            //ASSERT
            Assert.Equal(novaQuantidade, pedido.PedidoItens.FirstOrDefault(z => z.ProdutoId == produtoId).Quantidade);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());  
            var produtoId = Guid.NewGuid();          
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);
            
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 15);
            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                              pedidoItemAtualizado.Quantidade * pedidoItemExistente2.ValorUnitario;

            //ACT 
            pedido.AtualizarItem(pedidoItemAtualizado);

            //ASSERT
            Assert.Equal(totalPedido, pedido.ValorTotal);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade acima do Permitido")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItensUnidadeAcimaDoPermitido_DeveRetornarException()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());  
            var produtoId = Guid.NewGuid();          
            var pedidoItemExistente1 = new PedidoItem(produtoId, "Produto Teste", 3, 15);            
            pedido.AdicionarItem(pedidoItemExistente1);            
            
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 15);
            
            //ACT  //ASSERT
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));            

        }

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void RemoverItemPedido_ItensNaoExisteNaLista_DeveRetornarException()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());  
            var produtoId = Guid.NewGuid();          
            var pedidoItemRemover = new PedidoItem(produtoId, "Produto Teste", 3, 15);            
            
            //ACT  //ASSERT
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemRemover));            

        }

        [Fact(DisplayName = "Remover Item Pedido Deve calcular valor total")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
        {
            //ARRANGE
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());  
            var produtoId = Guid.NewGuid();          
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);            
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

            //ACT  
            pedido.RemoverItem(pedidoItem1);

            //ASSERT
            Assert.Equal(totalPedido, pedido.ValorTotal);

        }
    }
}